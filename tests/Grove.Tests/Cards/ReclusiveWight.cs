namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class ReclusiveWight
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SacWight()
      {
        Battlefield(P1, "Reclusive Wight", "Grizzly Bears");

        RunGame(1);
        Equal(1, P1.Battlefield.Count);
      }

      [Fact]
      public void DoNotSacWight()
      {
        Battlefield(P1, "Reclusive Wight", "Island");

        RunGame(1);
        Equal(2, P1.Battlefield.Count);
      }
    }
  }
}