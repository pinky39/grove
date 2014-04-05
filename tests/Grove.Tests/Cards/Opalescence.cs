namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Opalescence
  {
    public class Ai :AiScenario
    {
      [Fact]
      public void WorshipBecomes44()
      {
        Hand(P1, "Opalescence");
        Battlefield(P1, "Worship", "Plains", "Plains", "Plains", "Plains");
        P2.Life = 4;
        
        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}