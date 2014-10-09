namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SoulOfRavnica
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw2Cards()
      {
        Library(P1, "Grizzly Bears", "Grizzly Bears", "Grizzly Bears");
        Battlefield(P1, "Soul of Ravnica", "Grizzly Bears", "Grizzly Bears", "Juggernaut", "Island", "Island", "Island", "Island", "Plains", "Plains", "Plains");

        RunGame(1);

        Equal(2, P1.Hand.Count);
      }
    }
  }
}
