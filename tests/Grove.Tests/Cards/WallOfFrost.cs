namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class WallOfFrost
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void MachineDoesNotUntapDuringNextUntap()
      {
        var machine = C("Thran War Machine");

        Battlefield(P1, machine, "Forest", "Forest", "Forest", "Forest");
        Battlefield(P2, C("Wall Of Frost"));

        RunGame(1);

        True(C(machine).Has().DoesNotUntap);
      }
    }
  }
}