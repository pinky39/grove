namespace Grove.AI.CombatRules
{
  using System.Collections.Generic;
  using Grove.Infrastructure;

  [Copyable]
  public class CombatRules
  {
    private readonly TrackableList<CombatRule> _combatRules;

    private CombatRules() {}

    public CombatRules(IEnumerable<CombatRule> combatRules)
    {
      _combatRules = new TrackableList<CombatRule>(combatRules);
    }

    public CombatAbilities GetAbilities()
    {
      var abilities = new CombatAbilities();

      foreach (var combatRule in _combatRules)
      {
        combatRule.Apply(abilities);
      }

      return abilities;
    }

    public void Add(CombatRule rule)
    {
      _combatRules.Add(rule);
    }

    public void Initialize(Card owningCard, Game game)
    {
      _combatRules.Initialize(game.ChangeTracker);

      foreach (var combatRule in _combatRules)
      {
        combatRule.Initialize(owningCard, game);
      }
    }
  }
}