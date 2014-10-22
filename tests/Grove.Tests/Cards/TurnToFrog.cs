namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TurnToFrog
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void TurnJuggernautThenBlockWithBear()
      {
        var juggernaut = C("Juggernaut");
        Battlefield(P1, juggernaut);

        Hand(P2, "Turn to Frog");
        Battlefield(P2, "Grizzly Bears", "Island", "Island");
        P2.Life = 5;

        RunGame(1);

        Equal(Zone.Graveyard, C(juggernaut).Zone);       
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void CastTurnToFrog()
      {
        var juggernaut = C("Juggernaut");
        var spell = C("Turn to Frog");

        Hand(P1, spell);
        Battlefield(P1, juggernaut);        

        Exec(
          At(Step.FirstMain)
          .Cast(spell, target: juggernaut)
          .Verify(() => True(C(juggernaut).Is().Artifact))
          );      
      }
    }
  }
}
