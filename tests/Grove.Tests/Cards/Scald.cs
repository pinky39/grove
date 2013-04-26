namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Scald
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal2Damage()
      {
        Hand(P1, "Shivan Dragon");
        Battlefield(P1, "Island", "Island", "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Scald");

        RunGame(3);

        Equal(18, P1.Life);
      }
    }
  }
}