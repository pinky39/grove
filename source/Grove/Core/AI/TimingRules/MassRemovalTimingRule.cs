namespace Grove.AI.TimingRules
{
  using System.Linq;

  public class MassRemovalTimingRule : TimingRule
  {
    private readonly EffectTag _removalTag;

    private MassRemovalTimingRule() {}

    public MassRemovalTimingRule(EffectTag removalTag)
    {
      _removalTag = removalTag;
    }

    private bool RemovalDependsOnToughness()
    {
      return _removalTag == EffectTag.DealDamage || _removalTag == EffectTag.ReduceToughness;
    }
    
    private bool StackHasInterestingSpells()
    {
      return Stack.TopSpellHas(EffectTag.IncreaseToughness) && RemovalDependsOnToughness();
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {                  
      if (StackHasInterestingSpells())
      {
        return true;
      }

      if (IsBeforeYouDeclareAttackers(p.Controller))
      {
        return p.Controller.Opponent.Battlefield.CreaturesThatCanBlock.Count() > 0;
      }

      if (IsAfterOpponentDeclaresAttackers(p.Controller))
      {
        return Combat.Attackers.Count() > 0;
      }     
      
      if (Stack.CanBeDestroyedByTopSpell(p.Card))
        return true;

      if (IsEndOfOpponentsTurn(p.Controller) && _removalTag != EffectTag.Humble)
      {
        return true;
      }

      return false;
    }
  }
}