namespace Grove.Tests.Infrastructure
{
  /// <summary>
  ///   Player1 and Player2 are controlled by scenario script.
  /// </summary>
  public abstract class PredefinedScenario : Scenario
  {
    protected PredefinedScenario() : base(
      player1ControlledByScript: true,
      player2ControlledByScript: true) {}
  }
}