namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class JasmineSeer
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain4Life()
      {
        Hand(P1, "Serra Avatar", "Serra Avatar");
        Battlefield(P1, "Jasmine Seer", "Plains", "Plains", "Plains");
        RunGame(2);

        Equal(24, P1.Life);
      }
    }
  }  
}