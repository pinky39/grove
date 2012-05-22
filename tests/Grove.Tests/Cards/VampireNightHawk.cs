namespace Grove.Tests.Cards
{
  using System.Linq;
  using Grove.Core.Zones;
  using Infrastructure;
  using Xunit;

  public class VampireNightHawk
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void BlockSlum()
      {
        var hawk = C("Vampire Nighthawk");

        Battlefield(P1, "Rumbling Slum", "Rumbling Slum");
        Battlefield(P2, hawk);
        P2.Life = 8;

        RunGame(maxTurnCount: 2);

        Equal(Zone.Graveyard, C(hawk).Zone);
        Equal(1, P1.Graveyard.Count(x => x.Name == "Rumbling Slum"));
      }
    }
  }
}