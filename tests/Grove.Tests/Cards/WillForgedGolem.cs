namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WillForgedGolem
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BirdsCannotBeTappedForConvokeAndMana()
      {
        Hand(P1, "Will-Forged Golem");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Birds of Paradise");

        RunGame(1);

        Equal(1, P1.Hand.Count);
      }
    }
  }
}
