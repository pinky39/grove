namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class IntoTheVoid
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ReturnCreatures()
      {
        P1.Life = 2;
        Hand(P1, "Into the Void");
        Battlefield(P1, "Island", "Island", "Island", "Island");
        Battlefield(P2, "Grizzly Bears", "Grizzly Bears");

        RunGame(2);

        Equal(0, P2.Battlefield.Count);
      }
    }
  }
}