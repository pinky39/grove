namespace Grove.Gameplay.Effects
{
  using Grove.Ai;
  using Targeting;

  public class ExileTargets : Effect
  {
    private readonly bool _controllerGainsLifeEqualToToughness;

    private ExileTargets() {}

    public ExileTargets(bool controllerGainsLifeEqualToToughness = false)
    {
      _controllerGainsLifeEqualToToughness = controllerGainsLifeEqualToToughness;
      Category = EffectCategories.Exile;
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