namespace Grove.Tests.Cards
{
  using Gameplay;
  using Infrastructure;
  using Xunit;

  public class CopperGnomes
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void PutEngineIntoPlay()
      {
        var engine = C("Wurmcoil Engine");

        Hand(P2, engine);
        Battlefield(P2, "Forest", "Forest", "Forest", "Forest", "Copper Gnomes");

        RunGame(2);

        Equal(Zone.Battlefield, C(engine).Zone);
      }
    }
  }
}