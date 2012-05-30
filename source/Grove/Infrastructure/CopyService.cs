using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace Grove.Infrastructure
{
  public class CopyService
  {
    private static readonly Dictionary<Type, ParameterlessCtor> ActivatorCache =
      new Dictionary<Type, ParameterlessCtor>();

    private static readonly Dictionary<Type, Autocopy> AutocopyCache = new Dictionary<Type, Autocopy>();
    private readonly Dictionary<object, object> _identityMap = new Dictionary<object, object>();
    private List<Action> _contributors;

    public T CopyRoot<T>(T obj)
    {
      _contributors = new List<Action>();
      var copy = CopyInternal(obj);

      foreach (var contributor in _contributors)
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
      var type = ProxyUtil.GetUnproxiedType(obj);

      ParameterlessCtor ctor;
      if (ActivatorCache.TryGetValue(type, out ctor) == false)
      {
        ctor = type.GetParameterlessCtor();

        if (ctor == null)
        {
          throw new InvalidOperationException(
            String.Format("Type {0} is marked with [Copyable] but is missing a parameterless constructor.", type));
        }

        ActivatorCache.Add(type, ctor);
      }

      return ctor();
    }

    private static Autocopy GetAutocopy(Type type)
    {
      Autocopy autocopy = null;

      if (AutocopyCache.TryGetValue(type, out autocopy) == false)
      {
        var attributes = type.GetCustomAttributes(typeof (CopyableAttribute), inherit: true);
        var isCopyable = attributes.Length > 0;

        if (isCopyable)
        {
          autocopy = new Autocopy(type);
        }

        AutocopyCache.Add(type, autocopy);
      }

      return autocopy;
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

    private object CopyAutomaticly(object original)
    {
      var type = original.GetType();
      var autocopy = GetAutocopy(type);

      if (autocopy != null)
      {
        var copy = CreateCopy(original);
        _identityMap.Add(original, copy);

        autocopy.Copy(original, copy, CopyInternal);
        return copy;
      }

      return null;
    }

    private object CopyManualy(object original)
    {
      if (original is ICopyable)
      {
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
        _contributors.Add(() =>
                          contributor.AfterMemberCopy(original));
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
        obj;
    }

    private object GetCopyFromCache(object obj)
    {
      object copy;
      if (_identityMap.TryGetValue(obj, out copy))
      {
        return copy;
      }
      return null;
    }

    #region Nested type: Autocopy

    private class Autocopy
    {
      private readonly List<FieldInfo> _fields;

      public Autocopy(Type type)
      {
        _fields = GetFields(type);
      }

      public void Copy(object original, object copy, Func<object, object> copier)
      {
        foreach (var field in _fields)
        {
          // do not copy field created by castle proxy
          if (field.Name == "__interceptors")
            continue;

          // do not copy event registrations
          if (field.FieldType == typeof (EventHandler))
            continue;

          CopyField(field, original, copy, copier);
        }
      }

      private static List<FieldInfo> GetFields(Type type)
      {
        var fields = new List<FieldInfo>();
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        fields.AddRange(type.GetFields(bindingFlags));

        var baseType = type.BaseType;
        while (baseType != null)
        {
          fields.AddRange(baseType.GetFields(bindingFlags));
          baseType = baseType.BaseType;
        }

        return fields;
      }

      private void CopyField(FieldInfo field, object org, object copy, Func<object, object> copier)
      {
        var value = field.GetValue(org);
        field.SetValue(copy, copier(value));
      }
    }

    #endregion
  }
}