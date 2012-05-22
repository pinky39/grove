namespace Grove.Tests.Infrastructure
{
  /// <summary>
  ///   Player1 and Player2 are controlled by scenario script.
  /// </summary>
  public abstract class PredifinedScenario : Scenario
  {
    protected PredifinedScenario() : base(
      player1ControlledByScript: true,
      player2ControlledByScript: true) {}
  }
}