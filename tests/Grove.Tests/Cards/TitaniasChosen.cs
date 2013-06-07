namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class TitaniasChosen
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutCounterOnChosen()
      {
        var chosen = C("Titania's Chosen");

        Hand(P1, "Grizzly Bears");
        Battlefield(P1, chosen, "Forest", "Forest");

        RunGame(1);

        Equal(2, C(chosen).Power);
        Equal(2, C(chosen).Toughness);
      }
    }
  }
}