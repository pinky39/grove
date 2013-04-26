namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class ArgothianEnchantress
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void DrawCard()
      {
        var armor = C("Blanchwood Armor");
        var enchantress = C("Argothian Enchantress");
        var bear = C("Grizzly Bears");

        Hand(P1, armor, enchantress);
        Battlefield(P1, bear);

        Exec(
          At(Step.FirstMain)
            .Cast(enchantress)
            .Cast(armor, target: bear)
            .Verify(() => Equal(1, P1.Hand.Count))
          );
      }
    }
  }
}