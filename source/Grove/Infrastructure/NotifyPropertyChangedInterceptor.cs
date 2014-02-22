namespace Grove.Infrastructure
{
  using System.ComponentModel;
  using System.Reflection;
  using Caliburn.Micro;
  using Castle.DynamicProxy;

  public interface INotifyPropertyChangedRaiser
  {
    void RaisePropertyChanged(string propertyName);
  }

  public class NotifyPropertyChangedInterceptor : IInterceptor
  {
    private PropertyChangedEventHandler _propertyChangedSubscribers = delegate { };

    public void Intercept(IInvocation invocation)
    {
      if (InterceptNotifyPropertyChanged(invocation)) return;
      if (InterceptNotifyPropertyChangedRaiser(invocation)) return;
      if (InterceptCallsToProxiedType(invocation)) return;
    }

    private bool InterceptCallsToProxiedType(IInvocation invocation)
    {
      invocation.Proceed();

      var methodInfo = invocation.Method;

      if (methodInfo.IsGetter())
        return true;

      if (methodInfo.IsSetter())
      {
        var propertyName = invocation.Method.PropertyName();

        Execute.OnUIThread(() =>
          _propertyChangedSubscribers
            (invocation.InvocationTarget, new PropertyChangedEventArgs(propertyName)));
      }

      var memberToInspect = invocation.Method.IsSetter()
        ? (MemberInfo) invocation.Method.GetProperty()
        : invocation.Method;

      if (memberToInspect.HasAttribute<UpdatesAttribute>())
      {
        var updatesAttribute = memberToInspect.GetAttribute<UpdatesAttribute>();
        foreach (var propertyName in updatesAttribute.PropertyNames)
        {
          var propName = propertyName;

          Execute.OnUIThread(() => _propertyChangedSubscribers(
            invocation.Proxy, new PropertyChangedEventArgs(propName)));
        }
      }

      return true;
    }

    private bool InterceptNotifyPropertyChanged(IInvocation invocation)
    {
      if (invocation.Method.DeclaringType != typeof (INotifyPropertyChanged))
        return false;

      var propertyChangedEventHandler = (PropertyChangedEventHandler) invocation.Arguments[0];
      if (invocation.Method.IsAddEventHandler())
      {
        _propertyChangedSubscribers += propertyChangedEventHandler;
      }
      else
      {
        _propertyChangedSubscribers -= propertyChangedEventHandler;
      }
      return true;
    }

    private bool InterceptNotifyPropertyChangedRaiser(IInvocation invocation)
    {
      if (invocation.Method.DeclaringType != typeof (INotifyPropertyChangedRaiser))
        return false;

      var propertyName = (string) invocation.Arguments[0];
      Execute.OnUIThread(() =>
        _propertyChangedSubscribers(invocation.Proxy, new PropertyChangedEventArgs(propertyName)));

      return true;
    }
  }
}