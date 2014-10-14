namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RaiseTheAlarm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillAttackers()
      {
        Hand(P1, "Raise The Alarm");
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, "Savannah Lions", "Savannah Lions");

        RunGame(2);

        Equal(2, P2.Graveyard.Count);
      }
    }
  }
}