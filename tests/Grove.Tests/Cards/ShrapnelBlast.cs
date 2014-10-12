namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ShrapnelBlast
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal5DamageToPlayer()
      {
        Hand(P1, "Shrapnel Blast");
        Battlefield(P1, "Profane Memento", "Mountain", "Mountain", "Mountain", "Mountain");

        P2.Life = 5;

        RunGame(2);

        Equal(0, P2.Life);
      }

      [Fact]
      public void Deal5DamageToDragon()
      {
        Hand(P1, "Shrapnel Blast");
        Battlefield(P1, "Profane Memento", "Trained Armodon", "Trained Armodon", "Mountain", 
          "Mountain", "Mountain", "Mountain");
        
        Battlefield(P2, "Shivan Dragon");
        P2.Life = 6;

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}