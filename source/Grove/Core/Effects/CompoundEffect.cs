namespace Grove.Core.Effects
{
  using System.Collections.Generic;

  public class CompoundEffect : Effect
  {
    private readonly List<IEffectFactory> _effectFactories = new List<IEffectFactory>();

    public void ChildEffects(params IEffectFactory[] effectFactories)
    {
      _effectFactories.AddRange(effectFactories);      
    }

    protected override void ResolveEffect()
    {
      foreach (var effectFactory in _effectFactories)
      {
        var effect = effectFactory.CreateEffect(Source, X);
        effect.Target = Target;
        effect.Resolve();
      }
    }
  }
}