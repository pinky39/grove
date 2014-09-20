namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class DelusionsOfMediocrity
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void GainLife()
      {
        Hand(P1, "Delusions of Mediocrity");
        Battlefield(P1, "Island", "Island", "Island", "Island");

        RunGame(1);

        Equal(30, P1.Life);
      }

      [Fact]
      public void LooseLife()
      {
        Hand(P1, "Disenchant");
        Battlefield(P1, "Plains", "Plains");
        Battlefield(P2, "Delusions of Mediocrity");

        P2.Life = 10;
        
        RunGame(2);

        Equal(0, P2.Life);
      }
    }
  }
}