namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using Castle.DynamicProxy;

  public static class Bindable
  {
    private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

    public static T Create<T>(params object[] arguments)
    {
      return (T) Create(typeof (T), arguments);
    }

    private static object Create(Type type, params object[] arguments)
    {      
      return ProxyGenerator.CreateClassProxy(type, new[]{
        typeof (INotifyPropertyChanged),
        typeof (INotifyPropertyChangedRaiser),
        typeof (INotifyCollectionChanged),
      },ProxyGenerationOptions.Default, arguments, new NotifyPropertyChangedInterceptor());
    }
  }
}