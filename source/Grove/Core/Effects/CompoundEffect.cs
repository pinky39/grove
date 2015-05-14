namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;

  public class CompoundEffect : Effect
  {
    private readonly List<Effect> _childEffects = new List<Effect>();

    private CompoundEffect() {}

    public CompoundEffect(params Effect[] effects)
    {
      _childEffects.AddRange(effects);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return _childEffects.Sum(x => x.CalculateCreatureDamage(creature));
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return _childEffects.Sum(x => x.CalculatePlayerDamage(player));
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      var toughnessReduction = 0;

      foreach (var effect in _childEffects)
      {
        effect.Initialize(p, game, evaluateParameters);

        foreach (var effectTag in effect.GetTags())
        {
          SetTags(effectTag);
        }

        toughnessReduction = toughnessReduction + effect.ToughnessReduction.GetValue(X);
      }

      ToughnessReduction = toughnessReduction;

      return this;
    }

    public override void SetTriggeredAbilityTargets(Targets targets)
    {
      base.SetTriggeredAbilityTargets(targets);
      
      foreach (var childEffect in _childEffects)
      {
        childEffect.SetTriggeredAbilityTargets(targets);
      }
    }   

    public override void FinishResolve()
    {
      // calls after resolve hooks of child
      // effects.
      foreach (var effect in _childEffects)
      {
        effect.AfterResolve(new Context(this, Game));
      }

      base.FinishResolve();
    }

    protected override void ResolveEffect()
    {
      foreach (var effect in _childEffects)
      {
        effect.BeginResolve();
      }
    }
  }
}