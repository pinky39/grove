namespace Grove.Tests.Unit
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core;
  using Grove.Infrastructure;
  using Xunit;

  public class ManaFacts
  {
    [Fact]
    public void AmountEquivality()
    {
      var amount1 = Mana.Parse("{3}{GR}{RW}{W}{W}{B}");
      var amount2 = Mana.Parse("{3}{W}{RW}{GR}{B}{W}");
      var amount3 = Mana.Parse("{3}{W}{RW}{GR}{B}");

      Assert.Equal(amount1, amount2);
      Assert.NotEqual(amount1, amount3);
    }

    [Fact]
    public void Colorless()
    {
      var mana = new Mana();

      Assert.False(mana.IsColored);
      Assert.True(mana.IsColorless);
    }

    [Fact]
    public void Consume1()
    {
      var available = Pool("{R}{R}{R}{R}{G}{G}{G}");
      var amount = new ManaAmount("{4}{R}{R}");

      ManaPayment.Pay(amount, available);

      Assert.Equal(1, available.Amount.Count());
    }

    [Fact]
    public void Consume2()
    {
      var available = Pool("{B}{GRB}{R}");
      var amount = new ManaAmount("{G}{B}{R}");

      ManaPayment.Pay(amount, available);

      Assert.Equal(0, available.Amount.Count());
    }

    [Fact]
    public void Consume3()
    {
      var available = Pool("{U}{B}{GRB}{R}");
      var amount = new ManaAmount("{U}{G}");

      ManaPayment.Pay(amount, available);

      Assert.Equal(2, available.Amount.Count());
      Assert.Equal(new[]{Mana.Black, Mana.Red}, available.Amount.ToArray());
    }

    [Fact]
    public void DoesNotHave1()
    {
      var available = Mana.Parse("{R}{R}{R}{G}{G}{G}");
      var amount = Mana.Parse("{2}{R}{R}{R}{R}");

      HasNot(available, amount);
    }

    [Fact]
    public void DoesNotHave2()
    {
      var available = Mana.Parse("{2}{WG}{R}");
      var amount = Mana.Parse("{2}{W}{G}");

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
      var available = Mana.Parse("{R}{R}{R}{G}{G}{G}");
      var amount = Mana.Parse("{4}{R}{R}");

      Has(available, amount);
    }

    [Fact]
    public void Has2()
    {
      var available = Mana.Parse("{3}{RG}{R}{G}");
      var amount = Mana.Parse("{4}{R}{R}");

      Has(available, amount);
    }

    [Fact]
    public void Has3()
    {
      var available = Mana.Parse("{RG}{RG}");
      var amount = Mana.Parse("{G}{G}");

      Has(available, amount);
    }

    [Fact]
    public void Has4()
    {
      var available = Mana.Parse("{G}{RG}{RG}{G}");
      var amount = Mana.Parse("{2}{G}{G}");
      
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
      var parsed = Mana.Parse("{3}{B}{R}{RG}");

      var manaAmount = new ManaAmount(new[]{
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
      var manaCost = new ManaAmount("{5}{B}{G}{G}");
      var result = manaCost.GetSymbolNames();

      var expectedImageNames = new[]{
        "5", "b", "g", "g"
      };
      Assert.Equal(expectedImageNames, result);
    }

    [Fact]
    public void SymbolNamesZeroAmount()
    {
      var manaCost = ManaAmount.Zero;
      var result = manaCost.GetSymbolNames();
      Assert.Equal(new string[]{}, result);
    }

    private void Has(ManaAmount available, ManaAmount amount)
    {
      Assert.True(ManaPayment.CanPay(amount, new ManaSource(available).ToEnumerable()));
    }

    private void HasNot(ManaAmount available, ManaAmount amount)
    {
      Assert.False(ManaPayment.CanPay(amount, new ManaSource(available).ToEnumerable()));
    }

    private ManaSource Pool(string str)
    {
      return new ManaSource(Mana.Parse(str));
    }

    public class ManaSource : IManaSource
    {
      private readonly ManaBag _bag;

      public ManaSource(ManaAmount amount)
      {
        _bag = new ManaBag(amount);
      }

      public ManaAmount Amount { get { return GetAvailableMana(); } }
      public int Priority { get { return 1; } }
      object IManaSource.Resource { get { return this; } }

      public void Consume(ManaAmount amount)
      {
        _bag.Consume(amount);
      }

      public ManaAmount GetAvailableMana()
      {
        return _bag.Amount;
      }

      public void Consume(Dictionary<int, List<Mana>> payment)
      {
        Consume(new ManaAmount(payment[0]));
      }
    }
  }
}