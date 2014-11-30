namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SeraphOfTheMasses
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Is22()
      {
        var seraph = C("Seraph of the Masses");
        Hand(P1, seraph);
        Battlefield(P1, "Plains", "Plains", "Mountain", "Mountain", "Mountain", "Mountain", "Grizzly Bears");
        Battlefield(P2, "Coral Barrier");

        RunGame(1);

        Equal(2, C(seraph).Power);
        Equal(2, C(seraph).Toughness);
        Equal(Zone.Battlefield, C(seraph).Zone);
      }
    }
  }
}