namespace Grove.Gameplay.Effects
{
  using Artifical;

  public class RegenerateOwner : Effect
  {
    public RegenerateOwner()
    {
      Tags(EffectTag.Regenerate);
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.HasRegenerationShield = true;
    }
  }
}