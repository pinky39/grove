namespace Grove.Effects
{
  using Grove.AI;

  public class ExileTargets : Effect
  {
    private readonly bool _targetControllerGainsLifeEqualToToughness;
    private readonly bool _effectControllerGainsLifeEqualToToughness;

    private ExileTargets() {}

    public ExileTargets(bool targetControllerGainsLifeEqualToToughness = false, bool effectControllerGainsLifeEqualToToughness = false)
    {
      _targetControllerGainsLifeEqualToToughness = targetControllerGainsLifeEqualToToughness;
      _effectControllerGainsLifeEqualToToughness = effectControllerGainsLifeEqualToToughness;

      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        if (_targetControllerGainsLifeEqualToToughness)
        {
          target.Card().Controller.Life += target.Card().Toughness.GetValueOrDefault();
        }

        if (_effectControllerGainsLifeEqualToToughness)
        {
          Controller.Life += target.Card().Toughness.GetValueOrDefault();
        }

        target.Card().Exile();
      }
    }
  }
}