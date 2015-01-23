namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class TempleOfSilence
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutDragonOnTop()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Temple Of Silence");
        Library(P1, dragon);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest", "Mountain", "Mountain");

        RunGame(3);

        Equal(Zone.Battlefield, C(dragon).Zone);
      }

      [Fact]
      public void PutDragonOnBottom()
      {
        var dragon = C("Shivan Dragon");

        Hand(P1, "Temple Of Silence");
        Library(P1, dragon);
        Battlefield(P1, "Forest", "Forest", "Forest", "Forest");

        RunGame(3);

        Equal(Zone.Library, C(dragon).Zone);
      }
    }
  }
}
