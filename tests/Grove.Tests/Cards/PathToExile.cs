namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PathToExile
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileDragonItsControllerSearchesLand()
      {
        var land = C("Forest");
        var dragon = C("Shivan Dragon");

        Library(P1, land);
        Battlefield(P1, dragon);

        P2.Life = 5;
        Hand(P2, "Path To Exile");
        Battlefield(P2, "Plains");

        RunGame(1);

        Equal(Zone.Exile, C(dragon).Zone);
        Equal(Zone.Battlefield, C(land).Zone);
      }
    }
  }
}
