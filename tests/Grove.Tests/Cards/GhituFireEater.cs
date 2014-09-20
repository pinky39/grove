namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class GhituFireEater
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantAndDamage()
      {

        Hand(P1, "Rancor");
        Battlefield(P1, "Ghitu Fire-Eater", "Forest");
        Battlefield(P2, "Wall of Junk");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}