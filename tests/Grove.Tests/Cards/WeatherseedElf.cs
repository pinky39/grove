namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WeatherseedElf
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveForestwalk()
      {
        Battlefield(P1, "Weatherseed Elf", "Ravenous Baloth");
        Battlefield(P2, "Forest", "Wall of Blossoms");

        P2.Life = 4;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}