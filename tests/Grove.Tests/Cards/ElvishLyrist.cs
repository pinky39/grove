namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ElvishLyrist
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyDirge()
      {
        var dirge = C("Discordant Dirge");

        Battlefield(P1, dirge);
        Battlefield(P2, "Elvish Lyrist", "Forest");

        RunGame(1);

        Equal(Zone.Graveyard, C(dirge).Zone);
      }
    }
  }
}