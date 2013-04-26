namespace Grove.Tests.Cards
{
  using System.Linq;
  using Gameplay.States;
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class MartialCoup
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void JustCreateTokens()
      {
        var coup = C("Martial Coup");
        var dragon = C("Shivan Dragon");

        Hand(P1, coup);
        Battlefield(P1, dragon, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(5, P1.Battlefield.Creatures.Count());
        Equal(Zone.Battlefield, C(dragon).Zone);
      }

      [Fact]
      public void DestroyOtherCreatures()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(7, P1.Battlefield.Creatures.Count());
        Equal(0, P2.Battlefield.Creatures.Count());
      }
    }


    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void JustCreateTokens()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(coup, x: 4)
            .Verify(() => Equal(5, P1.Battlefield.Creatures.Count()))
          );
      }

      [Fact]
      public void DestroyOtherCreatures()
      {
        var coup = C("Martial Coup");

        Hand(P1, coup);
        Battlefield(P1, "Grizzly Bears");
        Battlefield(P2, "Grizzly Bears");

        Exec(
          At(Step.FirstMain)
            .Cast(coup, x: 5)
            .Verify(() =>
              {
                Equal(5, P1.Battlefield.Creatures.Count());
                Equal(0, P2.Battlefield.Creatures.Count());
              })
          );
      }
    }
  }
}