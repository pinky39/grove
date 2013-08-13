namespace Grove.Tests.Cards
{
  using Gameplay.Zones;
  using Infrastructure;
  using Xunit;

  public class GoblinWelder
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void ExchangePitTrapAndEngine()
      {
        var trap = C("Pit Trap");
        var engine = C("Wurmcoil Engine");
        
        Battlefield(P1, trap, "Goblin Welder");        
        Graveyard(P1, engine);

        RunGame(1);

        Equal(Zone.Battlefield,C(engine).Zone);
        Equal(Zone.Graveyard,C(trap).Zone);
      }
    }
  }
}