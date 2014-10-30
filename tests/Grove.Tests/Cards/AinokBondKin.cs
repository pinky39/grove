namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class AinokBondKin
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void VebulidGains11()
      {
        var vebulid = C("Vebulid");
        var bear = C("Grizzly Bears");
        Hand(P1, vebulid, bear);
        Battlefield(P1, "Swamp", "Forest", "Swamp", "Ainok Bond-Kin");

        RunGame(1);

        Equal(3, P1.Battlefield.Creatures.Count());
        True(C(vebulid).Has().FirstStrike);
        False(C(bear).Has().FirstStrike);
      }
    }
  }
}
