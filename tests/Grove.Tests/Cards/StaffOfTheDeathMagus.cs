namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StaffOfTheDeathMagus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GetLives()
      {
        Hand(P1, "Swamp", "Child of Night");
        Battlefield(P1, "Swamp");

        Library(P2, "Swamp");
        Hand(P2, "Swamp", "Child of Night");
        Battlefield(P2, "Staff of the Death Magus", "Swamp");

        RunGame(2);

        Equal(22, P2.Life);
      }
    }
  }
}
