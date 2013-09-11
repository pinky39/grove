namespace Grove.Gameplay.Effects
{
  using Artifical;
  using Targeting;

  public class RegenerateTarget : Effect
  {
    public RegenerateTarget()
    {
      Tags(EffectTag.Regenerate);
    }

    protected override void ResolveEffect()
    {
      Target.Card().HasRegenerationShield = true;
    }
  }
}