namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AcademyRector
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SearchForRancorAttachToAnaconda()
      {
        var anaconda = C("Anaconda");
        var rancor = C("Rancor");

        Battlefield(P1, "Academy Rector", anaconda);        
        Library(P1, rancor);
        
        Hand(P2, "Shock");
        Battlefield(P2, "Mountain");

        P2.Life = 4;

        RunGame(1);

        Equal(C(anaconda), C(rancor).AttachedTo);
      }
    }
  }
}