namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Anthroplasm
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PumpAnthroplasm()
      {
        var anthroplasm = C("Anthroplasm");
        Battlefield(P1, "Island", "Island", "Island", "Island", anthroplasm);
        Battlefield(P2, "Grizzly Bears");

        RunGame(2);

        Equal(4, C(anthroplasm).Power);
      }
    }
  }
}