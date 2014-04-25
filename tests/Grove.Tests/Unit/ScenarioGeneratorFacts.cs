namespace Grove.Tests.Unit
{
  using Diagnostics;
  using Infrastructure;
  using Xunit;

  public class ScenarioGeneratorFacts : Scenario
  {
    [Fact]
    public void GenerateScenario()
    {
      Hand(P1, C("Shock"));
      Hand(P2, C("Forest"), C("Mountain"));

      Battlefield(P1, C("Grizzly Bears"), C("Forest"));
      Battlefield(P2, C("Llanowar Elves").IsEnchantedWith(C("Blanchwood Armor")));

      var scenarioGenerator = new ScenarioGenerator(Game);
      var scenario = scenarioGenerator.WriteScenarioToString();

      Assert.NotNull(scenario);
    }
  }
}