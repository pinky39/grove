namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class NecropolisFiend
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExileGraveyardToDestroyBears()
      {
        var demon = C("Necropolis Fiend").IsEnchantedWith("Pacifism");

        Battlefield(P1, "Grizzly Bears");
        
        P2.Life = 2;
        Battlefield(P2, "Swamp", "Mountain", "Mountain", "Mountain", demon);
        Graveyard(P2, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");

        RunGame(1);

        Equal(2, P2.Life);
      }
    }
  }
}
