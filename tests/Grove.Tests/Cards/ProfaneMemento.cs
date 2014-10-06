namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ProfaneMemento
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillBearGain1Life()
      {
        Hand(P1, "Lightning Strike");
        Battlefield(P1, "Grizzly Bears", "Profane Memento", "Mountain", "Mountain");
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(21, P1.Life);
      }
    }
  }
}