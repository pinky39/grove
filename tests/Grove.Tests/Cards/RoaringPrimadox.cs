namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class RoaringPrimadox
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBear()
      {
        var bear = C("Grizzly Bears");
        Battlefield(P1, "Roaring Primadox", bear);

        RunGame(1);
        Equal(Zone.Hand, C(bear).Zone);
      }
    }
  }
}