namespace Grove.Tests.Unit
{
  using Xunit;

  public class ReflectionFacts
  {
    [Fact]
    public void GetNestedInterface()
    {
      var factory = typeof (Dog).GetNestedType("IFactory");
      Assert.Equal(typeof (Dog.IFactory), factory);
    }
  }

  public class Dog
  {
    public interface IFactory
    {
      void Create(string name);
    }
  }
}