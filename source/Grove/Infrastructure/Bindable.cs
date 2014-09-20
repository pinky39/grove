namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Linq.Expressions;
  using Castle.DynamicProxy;

  public static class Bindable
  {
    private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

    public static BindingInformation<T> Changes<T>(this SourceInfo sourceInfo, T obj)
    {
      var bindingInformation = new BindingInformation<T> {SourceInfo = sourceInfo, Target = obj};
      return bindingInformation;
    }

    public static void Property<T, TReturn>(this BindingInformation<T> bindingInformation,
      Expression<Func<T, TReturn>> expression)
    {
      var targetPropertyName = ((MemberExpression) expression.Body).Member.Name;

      var raiser = bindingInformation.Target as INotifyPropertyChangedRaiser;
      if (raiser == null)
        return;

      var notifier = bindingInformation.SourceInfo.SourceObject as INotifyPropertyChanged;
      if (notifier == null)
        return;

      notifier.PropertyChanged += (s, e) =>
        {
          if (e.PropertyName == bindingInformation.SourceInfo.PropertyName)
          {
            var targetType = typeof (T);
            var targetProperty = targetType.GetProperty(targetPropertyName);

            if (targetProperty.CanWrite)
            {
              var sourceProperty = notifier.GetType().GetProperty(bindingInformation.SourceInfo.PropertyName);
              var sourceValue = sourceProperty.GetValue(bindingInformation.SourceInfo.SourceObject, null);
              targetProperty.SetValue(bindingInformation.Target, sourceValue, null);
              return;
            }

            raiser.RaisePropertyChanged(targetPropertyName);
          }
        };
    }

    public static SourceInfo Property<T, TReturn>(this T obj, Expression<Func<T, TReturn>> expression)
    {
      var body = (MemberExpression) expression.Body;

      return new SourceInfo
        {
          PropertyName = body.Member.Name,
          SourceObject = obj
        };
    }

    public static void Updates(this object obj, params string[] propertyNames)
    {
      var raiser = obj as INotifyPropertyChangedRaiser;
      if (raiser == null)
        return;

      foreach (var propertyName in propertyNames)
      {
        raiser.RaisePropertyChanged(propertyName);
      }
    }

    public static T Create<T>(params object[] arguments)
    {
      return (T) Create(typeof (T), arguments);
    }

    private static object Create(Type type, params object[] arguments)
    {
      return ProxyGenerator.CreateClassProxy(type, new[]
        {
          typeof (INotifyPropertyChanged),
          typeof (INotifyPropertyChangedRaiser),
          typeof (INotifyCollectionChanged),
        }, ProxyGenerationOptions.Default, arguments, new NotifyPropertyChangedInterceptor());
    }

    public class BindingInformation<T>
    {
      public SourceInfo SourceInfo { get; set; }
      public T Target { get; set; }
    }

    public class SourceInfo
    {
      public string PropertyName { get; set; }
      public object SourceObject { get; set; }
    }
  }
}