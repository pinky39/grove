namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class MasterOfPredicaments
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutVerdantForceIntoPlay()
      {
        var force = C("Verdant Force");
        Hand(P1, force, "Grizzly Bears");
        Battlefield(P1, "Master of Predicaments");
        
        RunGame(1);

        Equal(Zone.Battlefield, C(force).Zone);
      }

      [Fact]
      public void DoNotPutBearIntoPlay()
      {
        var bear = C("Grizzly Bears");
        Hand(P1, bear);
        Battlefield(P1, "Master of Predicaments");

        RunGame(1);

        Equal(Zone.Hand, C(bear).Zone);
      }

      [Fact]
      public void EmptyHandShouldNotGetBug()
      {
        Hand(P1);
        Battlefield(P1, "Master of Predicaments");

        RunGame(1);
      }
    }
  }
}