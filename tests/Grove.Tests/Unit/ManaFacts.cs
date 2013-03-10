namespace Grove.Tests.Unit
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Core;
  using Core.Mana;
  using Grove.Infrastructure;
  using Xunit;

  public class ManaFacts
  {
    [Fact]
    public void Colorless()
    {
      var mana = new ManaUnit();

      Assert.False(mana.IsColored);
      Assert.True(mana.IsColorless);
    }

    [Fact]
    public void Performance()
    {
      var amount = "{4}{R}{R}".ParseMana();
      var manaSources = ManaSources(
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
        manaSources.Has(amount, ManaUsage.Any);
      }

      stopwatch.Stop();

      Console.WriteLine("{0} iterations took: {1}ms", iterations, stopwatch.Elapsed.TotalMilliseconds);
    }

    [Fact]
    public void Consume1()
    {
      var available = Mutable("{R}{R}{R}{R}{G}{G}{G}");
      var amount = "{4}{R}{R}".ParseMana();

      ManaPayment.Pay(amount, available);

      Assert.Equal(1, available.GetAvailableMana().Count());
    }

    [Fact]
    public void Consume2()
    {
      var available = Mutable("{B}{GRB}{R}");
      var amount = "{G}{B}{R}".ParseMana();

      ManaPayment.Pay(amount, available);

      Assert.Equal(0, available.GetAvailableMana().Count());
    }

    [Fact]
    public void Consume3()
    {
      var available = Mutable("{U}{B}{GRB}{R}");
      var amount = "{U}{G}".ParseMana();

      ManaPayment.Pay(amount, available);

      Assert.Equal(2, available.GetAvailableMana().Count());
      Assert.Equal(new[] {ManaUnit.Black, ManaUnit.Red}, available.GetAvailableMana().ToArray());
    }

    [Fact]
    public void DoesNotHave1()
    {
      var available = "{R}{R}{R}{G}{G}{G}".ParseMana();
      var amount = "{2}{R}{R}{R}{R}".ParseMana();

      AssertHasNot(available, amount);
    }

    [Fact]
    public void DoesNotHave2()
    {
      var available = "{2}{WG}{R}".ParseMana();
      var amount = "{2}{W}{G}".ParseMana();

      AssertHasNot(available, amount);
    }

    [Fact]
    public void Equality()
    {
      Assert.Equal(new ManaUnit(ManaColors.Red), new ManaUnit(ManaColors.Red));
    }

    [Fact]
    public void Has1()
    {
      var available = "{R}{R}{R}{G}{G}{G}".ParseMana();
      var amount = "{4}{R}{R}".ParseMana();

      AssertHas(available, amount);
    }

    [Fact]
    public void Has2()
    {
      var available = "{3}{RG}{R}{G}".ParseMana();
      var amount = "{4}{R}{R}".ParseMana();

      AssertHas(available, amount);
    }

    [Fact]
    public void Has3()
    {
      var available = "{RG}{RG}".ParseMana();
      var amount = "{G}{G}".ParseMana();

      AssertHas(available, amount);
    }

    [Fact]
    public void Has4()
    {
      var available = "{G}{RG}{RG}{G}".ParseMana();
      var amount = "{2}{G}{G}".ParseMana();

      AssertHas(available, amount);
    }

    [Fact]
    public void Has5()
    {
      var available = "{R}".ParseMana();
      var amount = 1.Colorless();

      AssertHas(available, amount);
    }

    [Fact]
    public void Multicolor()
    {
      var mana = new ManaUnit(ManaColors.Red | ManaColors.Green | ManaColors.Black);
      Assert.True(mana.IsColored);
      Assert.Equal(3, mana.Rank);
      Assert.True(mana.HasColor(ManaColors.Red));
      Assert.True(mana.HasColor(ManaColors.Green));
      Assert.True(mana.HasColor(ManaColors.Black));
    }

    [Fact]
    public void Parse()
    {
      var parsed = "{3}{B}{R}{RG}".ParseMana();

      var manaAmount = new PrimitiveManaAmount(new[]
        {
          new ManaUnit(),
          new ManaUnit(),
          new ManaUnit(),
          new ManaUnit(ManaColors.Black),
          new ManaUnit(ManaColors.Red),
          new ManaUnit(ManaColors.Green | ManaColors.Red)
        });

      Assert.Equal(manaAmount, parsed);
    }

    [Fact]
    public void SingleColorMana()
    {
      var mana = ManaUnit.Blue;

      Assert.True(mana.HasColor(ManaColors.Blue));
      Assert.True(mana.IsColored);
    }

    [Fact]
    public void SymbolNames1()
    {
      var manaCost = "{5}{B}{G}{G}".ParseMana();
      var result = manaCost.GetSymbolNames();

      var expectedImageNames = new[]
        {
          "5", "B", "G", "G"
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

    private ManaSources ManaSources(IEnumerable<IManaSource> manaSources)
    {
      var source = new ManaSources();

      foreach (var manaSource in manaSources)
      {
        source.Add(manaSource);
      }

      return source;
    }

    private bool Has(IManaAmount available, IManaAmount amount)
    {
      var manaSources = ManaSources(new MutableSource(available).ToEnumerable());
      return manaSources.Has(amount, ManaUsage.Any);
    }

    private void AssertHas(IManaAmount available, IManaAmount amount)
    {
      Assert.True(Has(available, amount));
    }

    private void AssertHasNot(IManaAmount available, IManaAmount amount)
    {
      Assert.False(Has(available, amount));
    }

    private IManaSource Mutable(string str)
    {
      return new MutableSource(str.ParseMana());
    }

    private IManaSource Immutable(string str)
    {
      return new ImmutableSource(str.ParseMana());
    }

    public class ImmutableSource : IManaSource
    {
      private readonly IManaAmount _manaAmount;

      public ImmutableSource(IManaAmount manaAmount)
      {
        _manaAmount = manaAmount;
      }

      public int Priority { get { return 0; } }

      public object Resource { get { return null; } }

      public void Consume(IManaAmount amount, ManaUsage usage)
      {
        throw new NotImplementedException();
      }

      public IManaAmount GetAvailableMana(ManaUsage usage)
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

      public IManaAmount Amount { get { return GetAvailableMana(ManaUsage.Any); } }

      public int Priority { get { return 1; } }

      object IManaSource.Resource { get { return this; } }

      public void Consume(IManaAmount amount, ManaUsage usage)
      {
        _bag.Consume(amount);
      }

      public IManaAmount GetAvailableMana(ManaUsage usage)
      {
        return _bag.GetAmount();
      }

      public void Consume(Dictionary<int, List<ManaUnit>> payment)
      {
        Consume(new PrimitiveManaAmount(payment[0]), ManaUsage.Any);
      }
    }
  }
}