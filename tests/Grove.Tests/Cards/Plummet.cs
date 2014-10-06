namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Plummet
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyKitefins()
      {
        var kitefins = C("Kapsho Kitefins");

        Hand(P2, "Plummet");
        Battlefield(P2, "Forest", "Forest");
        Battlefield(P1, kitefins, "Blood Host");

        P2.Life = 6;
        RunGame(1);

        Equal(Zone.Graveyard, C(kitefins).Zone);
      }
    }
  }
}