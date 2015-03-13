namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Hydrosurge
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReduceDragonsPower()
      {
        var dragon1 = C("Shivan Dragon");
        var dragon2 = C("Shivan Dragon");

        Hand(P1, "Hydrosurge");
        Battlefield(P1, dragon1, "Island");        
        Battlefield(P2, dragon2);

        RunGame(1);

        Equal(Zone.Battlefield, C(dragon1).Zone);
        Equal(Zone.Graveyard, C(dragon2).Zone);
      }
    }
  }
}