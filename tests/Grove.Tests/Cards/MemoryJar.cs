namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MemoryJar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReplaceHandWithNewOneUntilEot()
      {
        var dragon = C("Shivan Dragon");
        var hermit = C("Deranged Hermit");
        
        Hand(P1, dragon);                
        Library(P1, "Forest", "Forest", hermit, "Forest", "Forest");

        Battlefield(P1, "Memory Jar", "Forest", "Forest", "Forest", "Forest"); 

        RunGame(1);

        Equal(1, P1.Hand.Count);
        Equal(Zone.Battlefield, C(hermit).Zone);
        
      }
    }
  }
}