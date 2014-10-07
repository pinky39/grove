namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class Aetherspouts
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnBearsToLibrary()
      {
        Battlefield(P1, "Grizzly Bears", "Grizzly Bears");

        P2.Life = 4;
        Battlefield(P2, "Island", "Island", "Island", "Island", "Island", "Island");
        Hand(P2, "Aetherspouts");

        RunGame(1);

        Equal(0, P1.Battlefield.Count);
        Equal(62, P1.Library.Count);
      }
    }
  }
}
