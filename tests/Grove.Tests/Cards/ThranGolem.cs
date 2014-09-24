namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ThranGolem
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void EnchantGolem()
      {
          Hand(P1, "Rancor", "Rancor");
          Battlefield(P1, "Thran Golem", "Forest", "Forest");
          Battlefield(P2, "Wall of Blossoms");
          RunGame(1);

          Equal(11, P2.Life);
      }
    }
  }
}