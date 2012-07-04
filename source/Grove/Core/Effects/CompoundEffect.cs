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
        
        // this should be changed if compund effect would ever need more 
        // than one target
        effect.AddTarget(Target);
        
        effect.Resolve();                
      }
    }
  }
}