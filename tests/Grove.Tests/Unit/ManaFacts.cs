namespace Grove.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Core;
  using Grove.Infrastructure;
  using Xunit;

  public class ManaFacts
  {
    [Fact]
    public void Colorless()
    {
      var mana = new Mana();

      Assert.False(mana.IsColored);
      Assert.True(mana.IsColorless);
    }

    [Fact]
    public void Performance()
    {
      IManaAmount amount = "{4}{R}{R}".ParseManaAmount();
      var manaSources = new ManaSources(
        new[]
          {
            Immutable("{RG}{G}"),
            Immutable("{RG}"),
            Immutable("{R}"),
            Immutable("{BR}"),
            Immutable("{R}"),
            Immutable("{R}"),
            Immutable("{R}"),
            Immutable("{R}"),
            Immutable("{R}"),
            Immutable("{R}"),
          });

      var stopwatch = new Stopwatch();

      var iterations = 10000;

      stopwatch.Start();

      for (var i = 0; i < iterations; i++)
      {
        manaSources.Has(amount);
      }

      stopwatch.Stop();

      Console.WriteLine("{0} iterations took: {1}ms", iterations, stopwatch.Elapsed.TotalMilliseconds);
    }

    [Fact]
    public void Consume1()
    {
      var available = Mutable("{R}{R}{R}{R}{G}{G}{G}");
      var amount = "{4}{R}{R}".ParseManaAmount();

      ManaPayment.Pay(amount, available);

      Assert.Equal(1, available.GetAvailableMana().Count());
    }

    [Fact]
    public void Consume2()
    {
      var available = Mutable("{B}{GRB}{R}");
      var amount = "{G}{B}{R}".ParseManaAmount();

      ManaPayment.Pay(amount, available);

      Assert.Equal(0, available.GetAvailableMana().Count());
    }

    [Fact]
    public void Consume3()
    {
      var available = Mutable("{U}{B}{GRB}{R}");
      var amount = "{U}{G}".ParseManaAmount();

      ManaPayment.Pay(amount, available);

      Assert.Equal(2, available.GetAvailableMana().Count());
      Assert.Equal(new[] { Mana.Black, Mana.Red }, available.GetAvailableMana().ToArray());
    }

    [Fact]
    public void DoesNotHave1()
    {
      var available = "{R}{R}{R}{G}{G}{G}".ParseManaAmount();
      var amount = "{2}{R}{R}{R}{R}".ParseManaAmount();

      HasNot(available, amount);
    }

    [Fact]
    public void DoesNotHave2()
    {
      var available = "{2}{WG}{R}".ParseManaAmount();
      var amount = "{2}{W}{G}".ParseManaAmount();

      HasNot(available, amount);
    }

    [Fact]
    public void Equality()
    {
      Assert.Equal(new Mana(ManaColors.Red), new Mana(ManaColors.Red));
    }

    [Fact]
    public void Has1()
    {
      var available = "{R}{R}{R}{G}{G}{G}".ParseManaAmount();
      var amount = "{4}{R}{R}".ParseManaAmount();

      Has(available, amount);
    }

    [Fact]
    public void Has2()
    {
      var available = "{3}{RG}{R}{G}".ParseManaAmount();
      var amount = "{4}{R}{R}".ParseManaAmount();

      Has(available, amount);
    }

    [Fact]
    public void Has3()
    {
      var available = "{RG}{RG}".ParseManaAmount();
      var amount = "{G}{G}".ParseManaAmount();

      Has(available, amount);
    }

    [Fact]
    public void Has4()
    {
      var available = "{G}{RG}{RG}{G}".ParseManaAmount();
      var amount = "{2}{G}{G}".ParseManaAmount();

      Has(available, amount);
    }

    [Fact]
    public void Multicolor()
    {
      var mana = new Mana(ManaColors.Red | ManaColors.Green | ManaColors.Black);
      Assert.True(mana.IsColored);
      Assert.Equal(3, mana.Rank);
      Assert.True(mana.HasColor(ManaColors.Red));
      Assert.True(mana.HasColor(ManaColors.Green));
      Assert.True(mana.HasColor(ManaColors.Black));
    }

    [Fact]
    public void Parse()
    {
      var parsed = "{3}{B}{R}{RG}".ParseManaAmount();

      var manaAmount = new PrimitiveManaAmount(new[]
        {
          new Mana(),
          new Mana(),
          new Mana(),
          new Mana(ManaColors.Black),
          new Mana(ManaColors.Red),
          new Mana(ManaColors.Green | ManaColors.Red)
        });

      Assert.Equal(manaAmount, parsed);
    }

    [Fact]
    public void SingleColorMana()
    {
      var mana = Mana.Blue;

      Assert.True(mana.HasColor(ManaColors.Blue));
      Assert.True(mana.IsColored);
    }

    [Fact]
    public void SymbolNames1()
    {
      var manaCost = "{5}{B}{G}{G}".ParseManaAmount();
      var result = manaCost.GetSymbolNames();

      var expectedImageNames = new[]
        {
          "5", "b", "g", "g"
        };
      Assert.Equal(expectedImageNames, result);
    }

    [Fact]
    public void SymbolNamesZeroAmount()
    {
      var manaCost = ManaAmount.Zero;
      var result = manaCost.GetSymbolNames();
      Assert.Equal(new string[] {}, result);
    }

    private void Has(IManaAmount available, IManaAmount amount)
    {
      Assert.True(ManaPayment.CanPay(amount, new MutableSource(available).ToEnumerable()));
    }

    private void HasNot(IManaAmount available, IManaAmount amount)
    {
      Assert.False(ManaPayment.CanPay(amount, new MutableSource(available).ToEnumerable()));
    }

    private IManaSource Mutable(string str)
    {
      return new MutableSource(str.ParseManaAmount());
    }

    private IManaSource Immutable(string str)
    {
      return new ImmutableSource(str.ParseManaAmount());
    }

    #region Nested type: ManaSource

    public class ImmutableSource :IManaSource
    {
      private readonly IManaAmount _manaAmount;

      public ImmutableSource(IManaAmount manaAmount)
      {
        _manaAmount = manaAmount;
      }

      public int Priority
      {
        get { return 0; }
      }

      public object Resource
      {
        get { return null; }
      }

      public void Consume(IManaAmount amount)
      {
        throw new NotImplementedException();
      }

      public IManaAmount GetAvailableMana()
      {
        return _manaAmount;
      }
    }
    
    public class MutableSource : IManaSource
    {
      private readonly ManaBag _bag;

      public MutableSource(IManaAmount amount)
      {
        _bag = new ManaBag(amount);
      }

      public IManaAmount Amount
      {
        get { return GetAvailableMana(); }
      }

      #region IManaSource Members

      public int Priority
      {
        get { return 1; }
      }

      object IManaSource.Resource
      {
        get { return this; }
      }

      public void Consume(IManaAmount amount)
      {
        _bag.Consume(amount);
      }

      public IManaAmount GetAvailableMana()
      {
        return _bag.Amount;
      }

      #endregion

      public void Consume(Dictionary<int, List<Mana>> payment)
      {
        Consume(new PrimitiveManaAmount(payment[0]));
      }
    }

    #endregion
  }
}