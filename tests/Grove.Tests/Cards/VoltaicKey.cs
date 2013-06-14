namespace Grove.Tests.Cards
{
  using Infrastructure;
  using Xunit;

  public class VoltaicKey
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void UntapEngine()
      {
        var engine = C("Wurmcoil Engine");
        Battlefield(P1, "Voltaic Key", engine, "Swamp");
        Battlefield(P2, C("Child of Gaea").Tap(), "Forest", "Forest");

        RunGame(2);

        Equal(14, P2.Life);
        Equal(26, P1.Life);        
        False(C(engine).IsTapped);
      }
    }
  }
}