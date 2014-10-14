namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class StaffOfTheDeathMagus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Gain2Life()
      {                
        Hand(P1, "Swamp", "Child of Night");
        Battlefield(P1, "Staff of the Death Magus", "Swamp");

        RunGame(1);

        Equal(22, P1.Life);
      }
    }
  }
}
