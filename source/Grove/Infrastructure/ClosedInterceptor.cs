namespace Grove.Infrastructure
{
  using System;
  using Castle.DynamicProxy;

  public interface IClosable
  {
    event EventHandler Closed;
    void Close();
  }

  public class ClosedInterceptor : IInterceptor
  {
    private EventHandler _closedSubscribers = delegate { };

    public void Intercept(IInvocation invocation)
    {
      switch (invocation.Method.Name)
      {
        case "Close":
          _closedSubscribers(invocation.InvocationTarget, EventArgs.Empty);
          _closedSubscribers = delegate { };
          break;
        case "add_Closed":
          {
            var propertyChangedEventHandler = (EventHandler) invocation.Arguments[0];
            _closedSubscribers += propertyChangedEventHandler;
          }
          break;
        case "remove_Closed":
          {
            var propertyChangedEventHandler = (EventHandler) invocation.Arguments[0];
            _closedSubscribers -= propertyChangedEventHandler;
          }
          break;
      }


      if (invocation.TargetType != null)
        invocation.Proceed();
    }
  }

  public static class ClosableEx
  {
    public static void Close(this object obj)
    {
      var closable = (IClosable) obj;
      closable.Close();   
    }
  }
}