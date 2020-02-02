namespace Grove.Effects
{
  using System.Linq;

  public class FerociousEffect : CompoundEffect
  {
    private int _feroucionIndex;
    private readonly bool _instead;

    private FerociousEffect() {}

    public FerociousEffect(Effect[] normal, Effect[] ferocious, bool instead = false)
      : base(normal.Concat(ferocious).ToArray())
    {
      _feroucionIndex = normal.Length;
      _instead = instead;
    }

    public override void FinishResolve()
    {
      if (IsFerocious())
      {
        
        if (!_instead)
        {
          base.FinishResolve();
        }
        else
        {
          foreach (var effect in ChildEffects.Skip(_feroucionIndex))
          {
            effect.AfterResolve(new Context(this, Game));
          }

          EffectFinishResolve();
        }
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
        if (!_instead)
        {
          base.ResolveEffect();
        }
        else
        {
          foreach (var effect in ChildEffects.Skip(_feroucionIndex))
          {
            effect.BeginResolve();
          }
        }
        
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
