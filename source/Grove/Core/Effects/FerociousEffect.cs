namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;

  public class FerociousEffect : Effect
  {
    private readonly List<Effect> _normalEffects = new List<Effect>();
    private readonly List<Effect> _ferociousEffects = new List<Effect>();
    private readonly List<Effect> _childEffects = new List<Effect>();    

    private FerociousEffect() {}

    public FerociousEffect(Effect[] normalEffects, Effect[] ferociousEffects)
    {
      _normalEffects.AddRange(normalEffects);
      _ferociousEffects.AddRange(ferociousEffects);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
//      var effects = IsFerocious() ? _ferociousEffects : _normalEffects;
      return _normalEffects.Sum(x => x.CalculateCreatureDamage(creature));
    }

    public override int CalculatePlayerDamage(Player player)
    {
//      var effects = IsFerocious() ? _ferociousEffects : _normalEffects;
      return _normalEffects.Sum(x => x.CalculatePlayerDamage(player));
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      var toughnessReduction = 0;

      foreach (var effect in _normalEffects)
      {
        effect.Initialize(p, game, evaluateParameters);

        foreach (var effectTag in effect.GetTags())
        {
          SetTags(effectTag);
        }

        toughnessReduction = toughnessReduction + effect.ToughnessReduction.GetValue(X);
      }

      ToughnessReduction = toughnessReduction;

      foreach (var effect in _ferociousEffects)
      {
        effect.Initialize(p, game, evaluateParameters);

        foreach (var effectTag in effect.GetTags())
        {
          SetTags(effectTag);
        }
      }

      return this;
    }

    public override void SetTriggeredAbilityTargets(Targets targets)
    {
      foreach (var childEffect in _normalEffects)
      {
        childEffect.SetTriggeredAbilityTargets(targets);
      }

      foreach (var childEffect in _ferociousEffects)
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
        effect.AfterResolve(effect);
      }

      base.FinishResolve();
    }

    protected override void ResolveEffect()
    {
      var effects = IsFerocious() ? _ferociousEffects : _normalEffects;      
      _childEffects.AddRange(effects);

      foreach (var effect in _childEffects)
      {
        effect.BeginResolve();
      }
    }

    private bool IsFerocious()
    {
      return Controller.Battlefield.Creatures.Any(x => x.Power >= 4);
    }
  }
}
