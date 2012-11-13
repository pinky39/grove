namespace Grove.Core.Cards.Effects
{
  using System.Collections.Generic;
  using System.Linq;

  public class CompoundEffect : Effect
  {
    private readonly List<Effect> _childEffects = new List<Effect>();
    private readonly List<IEffectFactory> _effectFactories = new List<IEffectFactory>();

    public void ChildEffects(params IEffectFactory[] effectFactories)
    {
      _effectFactories.AddRange(effectFactories);
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return _childEffects.Sum(x => x.CalculateCreatureDamage(creature));            
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return _childEffects.Sum(x => x.CalculatePlayerDamage(player));
    }    

    protected override void Init()
    {
      foreach (var effectFactory in _effectFactories)
      {
        _childEffects.Add(effectFactory.CreateEffect(
          new EffectParameters(
            source: Source,
            targets: GetAllTargets()), Game));
      }
    }

    protected override void ResolveEffect()
    {
      foreach (var effect in _childEffects)
      {
        effect.Resolve();
      }
    }
  }
}