namespace Grove.Gameplay.Effects
{
  using Artifical;
  using Targeting;

  public class RegenerateTarget : Effect
  {
    public RegenerateTarget()
    {
      Category = EffectCategories.Protector;
    }
    
    protected override void ResolveEffect()
    {
      Target.Card().HasRegenerationShield = true;
    }
  }
}