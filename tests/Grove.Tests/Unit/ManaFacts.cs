namespace Grove.Tests.Unit
{
  using Gameplay.Mana;
  using Xunit;

  public class ManaFacts
  {
    [Fact]
    public void Consume1()
    {
      Add("{R}{R}{R}{R}{G}{G}{G}");
      Consume("{4}{R}{R}");

      AssertHas(1);
    }

    [Fact]
    public void Consume2()
    {
      Add("{B}{GRB}{R}");
      Consume("{G}{B}{R}");

      AssertHas(0);
    }

    [Fact]
    public void Consume3()
    {
      Add("{U}{B}{GRB}{R}");
      Consume("{U}{G}");

      AssertHas(2);
    }

    [Fact]
    public void DoesNotHave1()
    {
      Add("{R}{R}{R}{G}{G}{G}");

      AssertNotAvailable("{2}{R}{R}{R}{R}");
    }

    [Fact]
    public void DoesNotHave2()
    {
      Add("{2}{WG}{R}");
      AssertNotAvailable("{2}{W}{G}");
    }

    [Fact]
    public void Has1()
    {
      Add("{R}{R}{R}{G}{G}{G}");
      AssertAvailable("{4}{R}{R}");
    }

    [Fact]
    public void Has2()
    {
      Add("{3}{RG}{R}{G}");
      AssertAvailable("{4}{R}{R}");
    }

    [Fact]
    public void Has3()
    {
      Add("{RG}{RG}");
      AssertAvailable("{G}{G}");
    }

    [Fact]
    public void Has4()
    {
      Add("{G}{RG}{RG}{G}");
      AssertAvailable("{2}{G}{G}");
    }

    [Fact]
    public void Has5()
    {
      Add("{R}");
      AssertAvailable("{1}");
    }

    [Fact]
    public void TapRestriction1()
    {
      var tapRestriction = new object();

      Add("{R}", tapRestriction);
      Add("{G}", tapRestriction);

      AssertNotAvailable("{2}");
    }

    private readonly ManaVault _vault = new ManaVault();

    private void Add(string manaAmount)
    {
      _vault.AddManaToPool(manaAmount.Parse(), ManaUsage.Any);
    }

    private void Add(string manaAmount, object tapRestriction)
    {
      foreach (var sameColor in manaAmount.Parse())
      {
        for (int i = 0; i < sameColor.Count; i++)
        {
          var unit = new ManaUnit(sameColor.Color, 1, tapRestriction: tapRestriction);
          _vault.Add(unit);
        }
      }      
    }

    private void Consume(string manaAmount)
    {
      _vault.Consume(manaAmount.Parse(), ManaUsage.Any);
    }

    private void AssertHas(int count)
    {
      var amount = _vault.GetAvailableMana(ManaUsage.Any);
      Assert.Equal(count, amount.Converted);
    }

    private void AssertAvailable(string amount)
    {
      var has = _vault.Has(amount.Parse(), ManaUsage.Any);
      Assert.True(has);
    }

    private void AssertNotAvailable(string amount)
    {
      var has = _vault.Has(amount.Parse(), ManaUsage.Any);
      Assert.False(has);
    }
  }
}