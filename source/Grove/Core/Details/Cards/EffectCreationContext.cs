namespace Grove.Core.Details.Cards
{
  using Dsl;
  using Effects;

  public class EffectCreationContext<TEffect> where TEffect : Effect
  {
    public TEffect Effect { get; private set; }
    public EffectParameters Parameters { get; private set; }
    public CardBuilder Builder { get; private set; }

    public EffectCreationContext(TEffect effect, EffectParameters parameters, CardBuilder builder)
    {
      Effect = effect;
      Parameters = parameters;
      Builder = builder;
    }
  }
}