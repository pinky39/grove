namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Ulcerate
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void KillHorror()
      {
        var horror = C("Skittering Horror");
        Battlefield(P1, horror);
        Hand(P2, "Ulcerate");
        Battlefield(P2, "Swamp");


        RunGame(1);
        Equal(Zone.Graveyard, C(horror).Zone);
        Equal(17, P2.Life);
      }
    }
  }
}