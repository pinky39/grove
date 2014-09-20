namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class PhyrexianArena
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void Draw()
      {
        Battlefield(P1, "Phyrexian Arena");
        RunGame(1);

        Equal(19, P1.Life);
        Equal(1, P1.Hand.Count); // starting player skips draw 1st turn
      }
    }
  }
}