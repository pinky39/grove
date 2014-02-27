namespace Grove.Effects
{
  using Grove.AI;

  public class RegenerateTarget : Effect
  {
    public RegenerateTarget()
    {
      SetTags(EffectTag.Regenerate);
    }

    protected override void ResolveEffect()
    {
      Target.Card().HasRegenerationShield = true;
    }
  }
}