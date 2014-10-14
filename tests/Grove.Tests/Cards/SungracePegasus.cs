namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class SungracePegasus
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void AttackAndGain1Life()
      {
        Battlefield(P1, "Sungrace Pegasus");
        Battlefield(P2, "Grizzly Bears");
        P2.Life = 1;

        RunGame(1);

        Equal(21, P1.Life);
        Equal(0, P2.Life);
      }
    }
  }
}