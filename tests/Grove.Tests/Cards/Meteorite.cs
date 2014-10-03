namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Meteorite
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Deal2Damage()
      {
        Hand(P1, "Meteorite");
        Battlefield(P1, "Island", "Island", "Island", "Island", "Island");

        Battlefield(P2, "Grizzly Bears");

        RunGame(2);

        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}