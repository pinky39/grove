namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;
  using Dsl;
  using Mana;

  public class LevelUp : TimingRule
  {
    public IManaAmount Cost;
    private LevelDefinition[] _levelDefinitions;

    public void LevelDefinitions(params LevelDefinition[] levelDefinitions)
    {
      _levelDefinitions = levelDefinitions;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
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

      var totalCostToNextLevel = new AggregateManaAmount(Enumerable.Repeat(Cost, costToNextLevel.Value));
      return p.Controller.HasMana(totalCostToNextLevel, ManaUsage.Abilities);
    }
  }
}