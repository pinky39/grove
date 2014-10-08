namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DiffusionSliver
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PayManaDoNotCounterSpell()
      {
        Battlefield(P1, "Diffusion Sliver", "Diffusion Sliver");

        P2.Life = 2;
        Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P2, "Doom blade");

        RunGame(1);

        Equal(0, P2.Hand.Count);
        Equal(1, P2.Life);
      }

      [Fact]
      public void CounterSpellWhenCannotPayMana()
      {
        Battlefield(P1, "Diffusion Sliver", "Diffusion Sliver");

        P2.Life = 2;
        Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp", "Swamp");
        Hand(P2, "Doom blade");

        RunGame(1);

        Equal(1, P2.Hand.Count);
        Equal(0, P2.Life);
      }
    }
  }
}
