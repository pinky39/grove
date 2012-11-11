namespace Grove.Core.Details.Cards
{
  using Effects;

  public class EffectCreationContext<TEffect> where TEffect : Effect
  {
    public EffectCreationContext(TEffect effect, EffectParameters parameters)
    {
      Effect = effect;
      Parameters = parameters;
    }

    public TEffect Effect { get; private set; }
    public EffectParameters Parameters { get; private set; }
  }
}