namespace Grove.Core.Effects
{
  using Targeting;

  public class ExileTargets : Effect
  {
    private readonly bool _controllerGainsLifeEqualToToughness;

    public ExileTargets(bool controllerGainsLifeEqualToToughness = false)
    {
      _controllerGainsLifeEqualToToughness = controllerGainsLifeEqualToToughness;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        if (_controllerGainsLifeEqualToToughness)
        {
          target.Card().Controller.Life += target.Card().Toughness.GetValueOrDefault();
        }

        target.Card().Exile();
      }
    }
  }
}