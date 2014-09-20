namespace Grove.Infrastructure
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;  
  using Castle.DynamicProxy;

  public interface ICopyable
  {
    void Copy(object original, CopyService copyService);
  }

  public class CopyableAttribute : Attribute { }

  public interface ICopyContributor
  {
    void AfterMemberCopy(object original);
  }
  
  public class CopyService
  {
    private static readonly TypeCache Cache = new TypeCache();
    private readonly Dictionary<object, object> _identityMap = new Dictionary<object, object>();
    private List<Action> _pendingContributors;

    public T CopyRoot<T>(T obj)
    {
      _pendingContributors = new List<Action>();
      var copy = CopyInternal(obj);

      foreach (var contributor in _pendingContributors)
      {
        contributor();
      }

      return (T) copy;
    }

    public T Copy<T>(T obj)
    {
      return (T) CopyInternal(obj);
    }

    private static object CreateCopy(object obj)
    {
      var ctor = Cache.GetCtor(obj.GetType());
      return ctor();
    }

    private object CopyCollection(object obj)
    {
      var list = obj as IList;

      if (list != null)
      {
        var copy = (IList) CreateCopy(obj);
        foreach (var item in list)
        {
          var itemCopy = CopyInternal(item);
          copy.Add(itemCopy);
        }
        return copy;
      }
      return null;
    }

    private object CopyDictionary(object obj)
    {
      var dic = obj as IDictionary;

      if (dic != null)
      {
        var copy = (IDictionary) CreateCopy(obj);
        foreach (DictionaryEntry item in dic)
        {
          var valueCopy = CopyInternal(item.Value);
          var keyCopy = CopyInternal(item.Key);
          copy.Add(keyCopy, valueCopy);
        }
        return copy;
      }
      return null;
    }

    private object CopyAutomaticly(object original)
    {
      var type = original.GetType();

      var copyHandler = Cache.GetCopyHandler(type);

      if (copyHandler != null)
      {
        Cache.AddCopyable(type);

        var copy = CreateCopy(original);
        _identityMap.Add(original, copy);

        copyHandler.Copy(original, copy, CopyInternal);
        return copy;
      }

      return null;
    }

    private object CopyManualy(object original)
    {
      if (original is ICopyable)
      {
        Cache.AddCopyable(original.GetType());

        var copy = (ICopyable) CreateCopy(original);

        _identityMap.Add(original, copy);
        copy.Copy(original, this);
        return copy;
      }
      return null;
    }

    private object CopyCopyable(object original)
    {
      var copy = CopyAutomaticly(original) ?? CopyManualy(original);

      var contributor = copy as ICopyContributor;

      if (contributor != null)
      {
        _pendingContributors.Add(
          () => contributor.AfterMemberCopy(original));
      }

      return copy;
    }

    private object CopyInternal(object obj)
    {
      if (obj == null)
        return null;

      return
        GetCopyFromCache(obj) ??
          CopyCopyable(obj) ??
            CopyCollection(obj) ??
              CopyDictionary(obj) ??
                obj;
    }

    private object GetCopyFromCache(object obj)
    {
      if (!Cache.IsCopyable(obj.GetType()))
        return null;

      object copy;
      if (_identityMap.TryGetValue(obj, out copy))
      {
        return copy;
      }
      return null;
    }

    private class CopyHandler
    {
      private readonly List<FieldDescriptor> _fields;

      public CopyHandler(Type type)
      {
        _fields = GetFields(type);
      }

      public void Copy(object source, object target, Func<object, object> copier)
      {
        foreach (var field in _fields)
        {
          field.Copy(source, target, copier);
        }
      }

      private static List<FieldDescriptor> GetFields(Type type)
      {
        var fields = new List<FieldDescriptor>();
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        var baseType = type;
        while (baseType != null)
        {
          fields.AddRange(
            baseType.GetFields(bindingFlags)
              .Where(x => x.Name != "__interceptors")
              .Where(x => x.FieldType != typeof (EventHandler))
              .Where(
                x => !(x.FieldType.IsGenericType && x.FieldType.GetGenericTypeDefinition() == typeof (EventHandler<>)))
              .Select(fieldInfo => new FieldDescriptor(fieldInfo)));
          baseType = baseType.BaseType;
        }
        return fields;
      }

      private class FieldDescriptor
      {
        public FieldDescriptor(FieldInfo fieldInfo)
        {
          Getter = fieldInfo.GetGetter();
          Setter = fieldInfo.GetSetter();
        }

        public Func<object, object> Getter { get; private set; }
        public Action<object, object> Setter { get; private set; }

        public void Copy(object source, object target, Func<object, object> copier)
        {
          Setter(target, copier(Getter(source)));
        }
      }
    }

    private class TypeCache
    {
      private readonly HashSet<Type> _copyableTypes = new HashSet<Type>();
      private readonly object _ctorLock = new object();
      private readonly Dictionary<Type, ParameterlessCtor> _ctors = new Dictionary<Type, ParameterlessCtor>();
      private readonly Dictionary<Type, CopyHandler> _handlers = new Dictionary<Type, CopyHandler>();
      private readonly object _handlersLock = new object();
      private readonly object _typesLock = new object();

      public CopyHandler GetCopyHandler(Type type)
      {
        CopyHandler copyHandler = null;

        lock (_handlersLock)
        {
          if (_handlers.TryGetValue(type, out copyHandler) == false)
          {
            var attributes = type.GetCustomAttributes(typeof (CopyableAttribute), inherit: true);
            var isCopyable = attributes.Length > 0;

            if (isCopyable)
            {
              copyHandler = new CopyHandler(type);
            }

            _handlers.Add(type, copyHandler);
          }
        }

        return copyHandler;
      }

      public bool IsCopyable(Type type)
      {
        lock (_typesLock)
        {
          return _copyableTypes.Contains(type);
        }
      }

      public void AddCopyable(Type type)
      {
        lock (_typesLock)
        {
          _copyableTypes.Add(type);
        }
      }

      public ParameterlessCtor GetCtor(Type type)
      {
        lock (_ctorLock)
        {
          ParameterlessCtor ctor;
          if (_ctors.TryGetValue(type, out ctor) == false)
          {
            ctor = type.GetParameterlessCtor();

            Asrt.True(ctor != null,
              String.Format("Type {0} is marked with [Copyable] but is missing a parameterless constructor.", type));                        

            _ctors.Add(type, ctor);
          }

          return ctor;
        }
      }
    }
  }
}