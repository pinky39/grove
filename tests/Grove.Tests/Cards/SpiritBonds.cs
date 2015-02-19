namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class SpiritBonds
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PlayCreatureGetToken()
      {
        Hand(P1, "Grizzly Bears");
        Battlefield(P1, "Spirit Bonds", "Plains", "Plains", "Forest");

        RunGame(1);
        Equal(1, P1.Battlefield.Creatures.Count(c => c.Is().Token));
      }
    }

    public class PredefinedAi : PredefinedAiScenario
    {
      [Fact]
      public void SaveDragon()
      {
        var dragon1 = C("Shivan Dragon");
        var dragon2 = C("Shivan Dragon");

        Battlefield(P1, dragon1);        
        Battlefield(P2, dragon2, "Spirit Bonds", "Accursed Spirit", "Plains", "Plains");

        P2.Life = 5;

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(dragon1),
          At(Step.SecondMain)
          .Verify(() =>
            {
              Equal(Zone.Graveyard, C(dragon1).Zone);
              True(C(dragon2).Has().Indestructible);
              Equal(Zone.Battlefield, C(dragon2).Zone);
            })
          );
      }
    }
  }
}
