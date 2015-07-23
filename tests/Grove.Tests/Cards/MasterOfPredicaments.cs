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

      [Fact]
      public void CastRestockWithoutPayingMana()
      {
        var restock = C("Restock");
        var bear1 = C("Grizzly Bears");
        var bear2 = C("Grizzly Bears");
        Hand(P1, restock);
        Graveyard(P1, bear1, bear2);
        Battlefield(P1, "Master of Predicaments");

        RunGame(1);

        Equal(Zone.Exile, C(restock).Zone);
        Equal(Zone.Hand, C(bear1).Zone);
        Equal(Zone.Hand, C(bear2).Zone);
      }      
    }

    public class PredefineAi : PredefinedAiScenario
    {
      [Fact]
      public void MaxXIs2WhenCastingCostIsNotPayed()
      {
        var master = C("Master of Predicaments");
        var blaze = C("Blaze");
        
        Hand(P2, blaze);
        Battlefield(P2, master, "Mountain", "Forest");

        P1.Life = 6;

        Exec(
          At(Step.CombatDamage, turn: 2)
            .Answer(true)); // Set wrong answer, blaze CC is 1. And is not > 4.

        Equal(0, P1.Life);
        Equal(2, Game.Turn.TurnCount);
      }
    }
  }
}