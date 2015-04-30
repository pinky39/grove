namespace Grove.AI.TimingRules
{
  using System.Collections.Generic;

  public class DefaultLevelUpTimingRule : TimingRule
  {
    private readonly ManaAmount _cost;
    private readonly List<LevelDefinition> _levelDefinitions = new List<LevelDefinition>();

    private DefaultLevelUpTimingRule() {}

    public DefaultLevelUpTimingRule(ManaAmount cost, params LevelDefinition[] levelDefinitions)
    {
      _cost = cost;
      _levelDefinitions.AddRange(levelDefinitions);
    }

    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      var level = p.Card.Level ?? 0;
      int? costToNextLevel = null;

      foreach (var definition in _levelDefinitions)
      {
        if (definition.Max == null)
          break;

        if (level < definition.Min)
        {
          costToNextLevel = definition.Min - level;
          break;
        }

        if (definition.Min <= level && definition.Max >= level)
        {
          costToNextLevel = definition.Max + 1 - level;
          break;
        }
      }

      if (costToNextLevel == null)
        return false;

      var totalCostToNextLevel = _cost.Repeat(costToNextLevel.Value);
      return p.Controller.HasMana(totalCostToNextLevel, ManaUsage.Abilities);
    }
  }
}