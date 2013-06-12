namespace Grove.Artifical.CombatRules
{
  using Gameplay;
  using Infrastructure;

  [Copyable]
  public class CombatRules
  {
    private readonly TrackableList<CombatRule> _combatRules = new TrackableList<CombatRule>();

    public CombatAbilities GetAbilities()
    {
      var abilities = new CombatAbilities();

      foreach (var combatRule in _combatRules)
      {
        combatRule.Apply(abilities);
      }

      return abilities;
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