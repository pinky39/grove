namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class LayWaste
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void DestroyLand()
      {
        Hand(P1, "Lay Waste");
        Battlefield(P1, "Mountain", "Mountain", "Mountain", "Mountain");
        Battlefield(P2, "Swamp", "Swamp", "Swamp", "Swamp");

        RunGame(1);

        Equal(3, P2.Battlefield.Count);
      }
    }
  }
}