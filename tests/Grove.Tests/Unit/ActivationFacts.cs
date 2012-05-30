using System;
using System.Diagnostics;
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


    [Fact]
    public void Performance()
    {
      var getter = _ageField.GetGetter();
      var setter = _ageField.GetSetter();
      
      var dog1 = new Dog();
      var dog2 = new Dog();
      
      var iterations = 100000;
      
      var stopWatch = new Stopwatch();
      stopWatch.Start();


      for (int i = 0; i < iterations; i++)
      {         
       setter(dog2, getter(dog1));
      }

      stopWatch.Stop();

      Console.WriteLine("Using delegates: {0}.", stopWatch.Elapsed.TotalMilliseconds);


      stopWatch.Reset();
      stopWatch.Start();

      for (int i = 0; i < iterations; i++)
      {
        _ageField.SetValue(dog2, _ageField.GetValue(dog1));
      }
     
      Console.WriteLine("Using reflection: {0}.", stopWatch.Elapsed.TotalMilliseconds);
    }

    public class Dog
    {
      private readonly int _age = 1;
      public int Age { get { return _age; } }
    }
  }
}