namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Morphling : AiScenario
  {
    [Fact]
    public void FlyingPump()
    {
      Battlefield(P1, "Morphling", "Island", "Island", "Island", "Island", "Island");
      Battlefield(P2, "Grizzly Bears");

      P2.Life = 5;

      RunGame(1);

      Equal(0, P2.Life);
    }
  }
}