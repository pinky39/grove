namespace Grove.Tests.Cards
{
  using System.Linq;
  using Infrastructure;
  using Xunit;

  public class RakshasaVizier
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void RakshasaGetsCounterForExiled()
      {
        var rakshasa = C("Rakshasa Vizier").IsEnchantedWith("Pacifism");

        Battlefield(P1, "Juggernaut");

        P2.Life = 2;
        Battlefield(P2, rakshasa, "Grizzly Bears", "Planar Void");

        RunGame(1);

        Equal(1, P2.Exile.Count());
        Equal(5, C(rakshasa).Power);
      }
    }
  }
}
