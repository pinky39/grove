using System.Reflection;
using Grove.Infrastructure;
using Xunit;

namespace Grove.Tests.Unit
{
  public class ActivationFacts
  {
    private readonly FieldInfo _ageField = typeof (Dog).GetField("_age", BindingFlags.Instance | BindingFlags.NonPublic);

    [Fact]
    public void GetField()
    {
      var getter = _ageField.GetGetter();
      var dog = new Dog();

      Assert.Equal(getter(dog), dog.Age);
    }

    [Fact]
    public void SetField()
    {
      var setter = _ageField.GetSetter();
      var dog = new Dog();

      setter(dog, 2);

      Assert.Equal(2, dog.Age);
    }

    public class Dog
    {
      private readonly int _age = 1;
      public int Age { get { return _age; } }
    }
  }
}