namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AjanisPridemate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayDragoonsAndBoostPridemate()
      {
        Hand(P1, "Radiant's Dragoons");
        Battlefield(P1, "Ajani's Pridemate", "Plains", "Plains", "Plains", "Plains");

        P2.Life = 3;

        RunGame(1);
        Equal(0, P2.Life);
      }
    }
  }
}