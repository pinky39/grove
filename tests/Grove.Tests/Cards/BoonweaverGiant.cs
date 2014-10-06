namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class BoonweaverGiant
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SearchForRancorAttachToGiant()
      {
        var giant = C("Boonweaver Giant");
        var rancor = C("Rancor");

        Hand(P1, giant);
        Battlefield(P1, "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains", "Plains");
        Library(P1, rancor);

        RunGame(1);

        Equal(C(giant), C(rancor).AttachedTo);
      }
    }
  }
}
