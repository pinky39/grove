namespace Grove.Artifical.TimingRules
{
  using System.Linq;
  using Gameplay;
  using Gameplay.States;

  public class TargetRemoval : TimingRule
  {
    private readonly bool _combatOnly;

    private TargetRemoval() {}

    public TargetRemoval(bool combatOnly = false)
    {
      _combatOnly = combatOnly;
    }

    private bool TargetCreatureRemoval(Card target, TimingRuleParameters p)
    {
      // remove blockers
      if (p.Controller.IsActive && Turn.Step == Step.BeginningOfCombat)
      {
        return target.CanBlock();
      }

      // remove attackers
      if (p.Controller.IsActive == false && Turn.Step == Step.DeclareAttackers)
      {
        return target.IsAttacker;        
      }

      if (_combatOnly)
        return false;

       // play as response to some spells
      if (Stack.TopSpell != null && Stack.TopSpell.Controller == p.Controller.Opponent &&
        Stack.TopSpell.HasCategory(EffectCategories.Protector | EffectCategories.ToughnessIncrease))
      {
        if (Stack.TopSpell.Targets.Count > 0)
        {
          return Stack.TopSpell.Targets.Any(trg => trg == target);
        }

        // e.g Nantuko Shade gives self a +1/+1 boost
        if (Stack.TopSpell.TargetsEffectSource)
        {
          return target == Stack.TopSpell.Source.OwningCard;          
        }
      }

      return false;
    }

    private bool TargetAuraRemoval(Card target, TimingRuleParameters p)
    {
      // remove combat auras
      if (Turn.Step == Step.DeclareBlockers && (target.AttachedTo.IsAttacker || target.AttachedTo.IsBlocker))
        return true;

      return false;
    }

    private bool EotRemoval(TimingRuleParameters p)
    {
      if (!p.Controller.IsActive && Turn.Step == Step.EndOfTurn && !_combatOnly)
        return true;

      return false;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      foreach (var target in p.Targets<Card>())
      {
        if (target.Is().Creature && TargetCreatureRemoval(target, p))
        {
          return true;
        }

        if (target.Is().Aura && target.AttachedTo != null && TargetAuraRemoval(target, p))
        {
          return true;
        }        
      }

      return EotRemoval(p);     
    }
  }
}