namespace Grove.Core.Controllers.Scenario
{
  using Targeting;

  public class ScenarioActivation
  {
    private readonly Targets _targets = new Targets();

    public Card Card { get; set; }
    public int? X { get; set; }
    public int AbilityIndex { get; set; }
    public bool PayKicker { get; set; }

    public Targets GetTargets()
    {
      return _targets;
    }

    public void Targets(params ITarget[] effectTargets)
    {
      foreach (var effectTarget in effectTargets)
      {
        if (effectTarget != null)
          _targets.AddEffect(effectTarget);
      }
    }

    public void CostTargets(params ITarget[] costTargets)
    {
      foreach (var costTarget in costTargets)
      {
        if (costTarget != null)
          _targets.AddCost(costTarget);
      }
    }
  }
}