namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class CripplingBlight
  {
    public class Predefined : AiScenario
    {
      [Fact]
      public void CannotBlock()
      {
        Hand(P1, "Crippling Blight");
        Battlefield(P1, "Grizzly Bears", "Swamp");

        P2.Life = 2;
        Battlefield(P2, "Grizzly Bears");

        RunGame(1);

        Equal(0, P2.Life);
      }
    }
  }
}