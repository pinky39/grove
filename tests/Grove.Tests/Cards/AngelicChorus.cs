namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class AngelicChorus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLife()
      {
        Hand(P1, "Grizzly Bears", "Elvish Warrior");
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Angelic Chorus");

        RunGame(1);

        Equal(25, P1.Life);
      }
    }
  }
}