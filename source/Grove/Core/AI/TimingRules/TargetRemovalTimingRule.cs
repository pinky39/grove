namespace Grove.AI.TimingRules
{
  using System.Collections.Generic;

  public class TargetRemovalTimingRule : TimingRule
  {
    private readonly bool _combatOnly;
    private readonly List<EffectTag> _removalTags = new List<EffectTag>();

    private TargetRemovalTimingRule() {}

    public TargetRemovalTimingRule(EffectTag? removalTag = null, bool combatOnly = false)
    {
      if (removalTag.HasValue)
      {
        _removalTags.Add(removalTag.Value);
      }

      _combatOnly = combatOnly;
    }

    public TargetRemovalTimingRule RemovalTags(params EffectTag[] removalTags)
    {
      _removalTags.AddRange(removalTags);
      return this;
    }

    private bool RemovalDependsOnToughness()
    {
      return _removalTags.Contains(EffectTag.DealDamage) || _removalTags.Contains(EffectTag.ReduceToughness);
    }

    private bool StackHasInterestingSpells()
    {
      if (Stack.TopSpellHas(EffectTag.Shroud))
        return true;

      if (Stack.TopSpellHas(EffectTag.IncreaseToughness) && RemovalDependsOnToughness())
        return true;

      return false;
    }

    private bool TargetCreatureRemoval(Card target, TimingRuleParameters p)
    {
      if (!target.Is().Creature)
        return false;

      if (IsBeforeYouDeclareAttackers(p.Controller))
      {
        return target.CanBlock();
      }

      if (IsBeforeYouDeclareBlockers(p.Controller))
      {
        return target.IsAttacker;
      }

      if (_combatOnly)
        return false;

      if (StackHasInterestingSpells())
      {
        if (Stack.TopSpell.HasEffectTargets())
        {
          return Stack.TopSpell.HasEffectTarget(target);
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
      if ((!target.Is().Aura && !target.Is().Equipment) || target.AttachedTo == null)
        return false;

      if (IsAfterOpponentDeclaresBlockers(p.Controller) && target.AttachedTo.IsBlocker)
      {
        return true;
      }

      if (IsAfterOpponentDeclaresAttackers(p.Controller) && target.AttachedTo.IsAttacker)
      {
        return true;
      }

      return false;
    }

    private bool EotRemoval(TimingRuleParameters p)
    {
      if (_combatOnly)
        return false;

      return IsEndOfOpponentsTurn(p.Controller);
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      // quick check before target generation      
      if (Stack.IsEmpty)
      {
        if (_removalTags.Contains(EffectTag.DealDamage) || _removalTags.Contains(EffectTag.CreaturesOnly) ||
          _removalTags.Contains(EffectTag.ReduceToughness) || _removalTags.Contains(EffectTag.Humble) || _removalTags.Contains(EffectTag.CombatDisabler))
        {
          return IsBeforeYouDeclareAttackers(p.Controller) || IsBeforeYouDeclareBlockers(p.Controller) ||
            IsEndOfOpponentsTurn(p.Controller);
        }

        return IsBeforeYouDeclareAttackers(p.Controller) || IsBeforeYouDeclareBlockers(p.Controller) ||
          IsAfterOpponentDeclaresAttackers(p.Controller) || IsAfterOpponentDeclaresAttackers(p.Controller) ||
            IsEndOfOpponentsTurn(p.Controller);
      }

      return StackHasInterestingSpells();
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      foreach (var target in p.Targets<Card>())
      {
        if (TargetCreatureRemoval(target, p))
        {
          return true;
        }

        if (TargetAuraRemoval(target, p))
        {
          return true;
        }
      }

      return EotRemoval(p);
    }
  }
}