namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class ShowerOfSparks
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillTheBirdDamageOpponent()
      {
        var bird = C("Birds of Paradise");

        Hand(P1, "Shower of Sparks");
        Battlefield(P1, "Mountain");        
        Battlefield(P2, bird);

        RunGame(1);

        Equal(Zone.Graveyard, C(bird).Zone);
        Equal(19, P2.Life);        
      }
    }
  }
}