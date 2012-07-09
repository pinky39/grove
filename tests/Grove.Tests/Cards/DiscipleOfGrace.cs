namespace Grove.Tests.Cards
{
  using Core;
  using Core.Zones;
  using Infrastructure;
  using Xunit;

  public class DiscipleOfGrace
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Cycle()
      {
        var dragon = C("Shivan Dragon");
        var disciple = C("Disciple of Grace");

        Hand(P1, disciple);
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, dragon);

        RunGame(2);

        Equal(Zone.Graveyard, C(disciple).Zone);
        Equal(1, P1.Hand.Count);
      }

      [Fact]
      public void Cast()
      {
        var titan = C("Grave Titan");
        var disciple = C("Disciple of Grace");

        Hand(P1, disciple);
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, titan);

        RunGame(2);

        Equal(Zone.Battlefield, C(disciple).Zone);
        Equal(20, P1.Life);
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void Cycle()
      {
        var disciple = C("Disciple of Grace");
        Hand(P1, disciple);

        Exec(
          At(Step.FirstMain)
            .Cycle(disciple)
            .Verify(() =>
              {
                Equal(Zone.Graveyard, C(disciple).Zone);
                Equal(1, P1.Hand.Count);
              })
          );
      }
    }
  }
}