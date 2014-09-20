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
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");
        Library(P1, rancor);                

        P2.Life = 1;

        RunGame(1);

        Equal(1, P2.Life);
        Equal(C(anaconda), C(rancor).AttachedTo);
      }
    }
  }
}