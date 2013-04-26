namespace Grove.Tests.Cards
{
  using Gameplay.States;
  using Infrastructure;
  using Xunit;

  public class FertileGround
  {
    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Add2ManaToManaPool()
      {
        var forest = C("Forest");

        Battlefield(P1, forest.IsEnchantedWith("Fertile Ground"));

        Exec(
          At(Step.FirstMain)
            .Verify(() => Equal(2, P1.GetConvertedMana()))
          );
      }

      [Fact]
      public void AvailableManaIs2()
      {
        var forest = C("Forest");

        Battlefield(P1, forest.IsEnchantedWith("Fertile Ground"));

        Exec(
          At(Step.FirstMain)
            .Verify(() => Equal(2, P1.GetConvertedMana()))
          );
      }

      [Fact]
      public void AvailableManaIs1()
      {
        var ground = C("Fertile Ground");
        var clear = C("Clear");

        Battlefield(P1, C("Forest").IsEnchantedWith(ground));
        Hand(P2, clear);

        Exec(
          At(Step.FirstMain)
            .Cast(clear, target: ground)
            .Verify(() => Equal(1, P1.GetConvertedMana()))
          );
      }
    }
  }
}