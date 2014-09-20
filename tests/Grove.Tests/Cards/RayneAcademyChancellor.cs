namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RayneAcademyChancellor
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw2Card()
      {
        Battlefield(P1, C("Rayne, Academy Chancellor").IsEnchantedWith("Rancor"));
        
        Hand(P2, "Shock");
        Battlefield(P2, "Mountain");
        P2.Life = 3;

        RunGame(1);

        Equal(3, P1.Hand.Count);
      }
    }
  }
}