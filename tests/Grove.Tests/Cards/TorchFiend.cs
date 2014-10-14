namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TorchFiend
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyCathodion()
      {
        var torchFiend = C("Torch Fiend");
        var cathodion = C("Cathodion");

        Battlefield(P1, torchFiend, "Mountain");
        Battlefield(P2, cathodion);

        RunGame(2);

        Equal(Zone.Graveyard, C(torchFiend).Zone);
        Equal(Zone.Graveyard, C(cathodion).Zone);
      }
    }
  }
}