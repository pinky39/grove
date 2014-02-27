namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Parch
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal4DamageToBlueCreature()
      {
        var serpent = C("Sandbar Serpent");
        
        Hand(P1, "Parch");
        Battlefield(P1, "Mountain", "Mountain");
        
        Battlefield(P2, "Grizzly Bears", serpent);

        RunGame(1);

        Equal(Zone.Graveyard, C(serpent).Zone);
      }
    }
  }
}