namespace Grove.Effects
{  
  using System.Linq;

  public class FerociousEffect : CompoundEffect
  {
    private int _feroucionIndex;

    private FerociousEffect() {}

    public FerociousEffect(Effect[] normal, Effect[] ferocious)
      : base(normal.Concat(ferocious).ToArray())
    {
      _feroucionIndex = normal.Length;
    }

    public override void FinishResolve()
    {
      if (IsFerocious())
      {
        base.FinishResolve();
        return;
      }
            
      foreach (var effect in ChildEffects.Take(_feroucionIndex))
      {
        effect.AfterResolve(new Context(this, Game));
      }

      EffectFinishResolve();
    }

    protected override void ResolveEffect()
    {
      if (IsFerocious())
      {
        base.ResolveEffect();
        return;
      }

      foreach (var effect in ChildEffects.Take(_feroucionIndex))
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
