namespace Grove.AI.CombatRules
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;

  public class CombatRules : GameObject, ICopyContributor
  {
    private readonly CardBase _cardBase;
    private readonly Characteristic<List<CombatRule>> _rules;

    private CombatRules() {}

    public CombatRules(CardBase cardBase)
    {
      _cardBase = cardBase;      
      _rules = new Characteristic<List<CombatRule>>(_cardBase.Value.CombatRules);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
    }

    public CombatAbilities GetAbilities()
    {
      var abilities = new CombatAbilities();

      foreach (var combatRule in _rules.Value)
      {
        combatRule.Apply(abilities);
      }

      return abilities;
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;

      _rules.Initialize(game, card);            
      _cardBase.Changed += OnCardBaseChanged;
    }

    private void OnCardBaseChanged()
    {
      _rules.ChangeBaseValue(_cardBase.Value.CombatRules);
    }
  }
}