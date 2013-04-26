namespace Grove.Tests.Infrastructure
{
  /// <summary>
  ///   Player1 is controlled by scenario script. Player2 is controlled by build in artifical inteligence algorithm.
  /// </summary>
  public abstract class PredefinedAiScenario : Scenario
  {
    protected PredefinedAiScenario() : base(
      player1ControlledByScript: true,
      player2ControlledByScript: false) {}
  }
}