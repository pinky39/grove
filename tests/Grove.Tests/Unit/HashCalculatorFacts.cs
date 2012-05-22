namespace Grove.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using Grove.Core;
  using Grove.Infrastructure;
  using Xunit;

  public class HashCalculatorFacts
  {
    [Fact]
    public void Calculate()
    {
      var hashcode1 = new HashCalculator().Calculate(new Car(3));
      var hashcode2 = new HashCalculator().Calculate(new Car(4));
      var hashcode3 = new HashCalculator().Calculate(new Car(4));

      Assert.NotEqual(hashcode1, hashcode2);
      Assert.Equal(hashcode2, hashcode3);
    }

    [Fact]
    public void Cache()
    {
      var calculator = new HashCalculator();
      var obj = new Country();

      var hashcode1 = calculator.Calculate(obj);
      var hashcode2 = calculator.Calculate(obj);

      Assert.Equal(hashcode1, hashcode2);
    }

    public class Country : IHashable
    {
      private static readonly Random Rnd = new Random();
      
      public int CalculateHash(HashCalculator hashCalculator)
      {
        return Rnd.Next();
      }
    }    

    public class Car : IHashable
    {
      public List<Wheel> Wheels = new List<Wheel>();

      public Car(int wheelCount)
      {
        for (var i = 0; i < wheelCount; i++)
        {
          Wheels.Add(new Wheel());
        }
      }

      public int CalculateHash(HashCalculator hashCalculator)
      {
        return hashCalculator.Calculate(Wheels);
      }
    }

    public class Wheel : IHashable
    {
      public int CalculateHash(HashCalculator hashCalculator)
      {
        return 1;
      }
    }
  }
}