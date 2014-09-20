namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class KarnSilverGolem
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ChangeToCreatureAndAttack()
      {
        var golem = C("Karn, Silver Golem");
        Battlefield(P1, "Urza's Armor", "Urza's Armor", golem, "Swamp", "Swamp");
        Battlefield(P2, "Llanowar Behemoth");

        RunGame(1);

        Equal(8, P2.Life);
        Equal(Zone.Battlefield, C(golem).Zone);
      }
    }
  }
}