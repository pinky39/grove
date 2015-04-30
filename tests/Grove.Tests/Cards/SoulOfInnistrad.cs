namespace Grove.Tests.Cards
{
  using System.Collections.Generic;
  using Infrastructure;
  using Xunit;
  using System.Linq;

  public class SoulOfInnistrad
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileFromGraveyardToReturn3CreaturesToHand()
      {
        var soul = C("Soul of Innistrad");
        Graveyard(P1, soul, "Shivan Dragon", "Shivan Dragon", "Shivan Dragon");
        Battlefield(P1, "Swamp", "Swamp", "Mountain", "Mountain", "Mountain");

        RunGame(2);

        Equal(3, P1.Hand.Count(x => x.Name == "Shivan Dragon"));
        Equal(Zone.Exile, C(soul).Zone);
      }  
    }       
  }
}