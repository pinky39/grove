namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class FrostLynx
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapCreature()
      {
        var host = C("Blood Host");
        
        Hand(P1, "Frost Lynx");
        Battlefield(P1, "Island", "Island", "Island");
        
        Battlefield(P2, host);
        RunGame(2);

        True(C(host).IsTapped);
      }
    }
  }
}