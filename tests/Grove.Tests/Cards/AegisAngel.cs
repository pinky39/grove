namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AegisAngel
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GiveAnthemIndestructible()
      {
        var anthem = C("Glorious Anthem");
        
        Hand(P1, "Aegis Angel");        
        Battlefield(P1, anthem, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");

        RunGame(1);

        True(C(anthem).Has().Indestructible);
      }
    }
  }
}