namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class KiorasFollower
  {
    public class Ai : AiScenario
    {

      [Fact]
      public void UntapAndBlock()
      {
        Battlefield(P1, "Trained Armodon");
        Battlefield(P2, "Kiora's Follower", C("Llanowar Elves").Tap());

        P2.Life = 3;

        RunGame(1);

        Equal(1, P2.Graveyard.Count(x => x.Name == "Llanowar Elves"));
        Equal(3, P2.Life);
      }
    }
  }
}