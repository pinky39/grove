namespace Grove.Tests.Performance
{
  using System;

  [Serializable]
  public class ScenarioRunResult
  {
    public readonly TimeSpan Duration;
    public readonly string Name;

    public ScenarioRunResult(string name, TimeSpan duration)
    {
      Name = name;
      Duration = duration;
    }
  }
}