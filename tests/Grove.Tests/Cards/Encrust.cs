namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Encrust
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TapAndDisableActivatedAbilities()
      {
        var dragon = C("Shivan Hellkite");
        
        Hand(P1, "Encrust");
        Battlefield(P1, "Island", "Island", "Island");        
        Battlefield(P2, dragon, "Mountain", "Mountain");

        P2.Life = 1;
        
        RunGame(2);

        Equal(1, P2.Life);
        True(C(dragon).IsTapped);
        
      }
    }
  }
}