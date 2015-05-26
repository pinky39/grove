namespace Grove.Tests.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Xunit;

  public class SoulOfShandalar
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DealDamageToPlayerAndCreature()
      {
        var armodon = C("Trained Armodon");

        Battlefield(P1, "Soul of Shandalar", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");        
        Battlefield(P2, armodon);
        
        P2.Life = 9;

        RunGame(1);

        Equal(Zone.Graveyard, C(armodon).Zone);
        Equal(0, P2.Life);
      }

      [Fact]
      public void DealDamageToPlayerOnly()
      {        
        Battlefield(P1, "Soul of Shandalar", "Mountain", "Mountain", "Mountain", "Mountain", "Mountain");
        
        P2.Life = 9;

        RunGame(2);        
        Equal(0, P2.Life);
      }
    }        
  }
}