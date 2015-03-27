namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PolymorphistsJest
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillDragons()
      {
        var dragon1 = C("Shivan Dragon");
        var dragon2 = C("Shivan Dragon");
        
        Battlefield(P1 , dragon1, dragon2);

        Hand(P2, "Polymorphist's Jest");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears", "Island", "Island", "Island");

        RunGame(1);

        Equal(Zone.Graveyard, C(dragon1).Zone);
        Equal(Zone.Graveyard, C(dragon2).Zone);
      }
    }
  }
}