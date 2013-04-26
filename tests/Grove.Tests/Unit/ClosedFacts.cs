namespace Grove.Tests.Unit
{
  using Castle.DynamicProxy;
  using Grove.Infrastructure;
  using Infrastructure;
  using Xunit;

  public class ClosedFacts : Scenario
  {
    [Fact]
    public void NotifyOnce()
    {
      var garbage = (IClosable) Proxy<Garbage>();

      var count = 0;
      garbage.Closed += delegate { count++; };

      ClosableEx.Close(garbage);
      ClosableEx.Close(garbage);

      Assert.Equal(1, count);
    }

    private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

    private T Proxy<T>()
    {
      return (T) ProxyGenerator.CreateClassProxy(typeof (T), new[]
        {
          typeof (IClosable),
        }, ProxyGenerationOptions.Default, new object[] {}, new ClosedInterceptor());
    }

    public class Garbage {}
  }
}