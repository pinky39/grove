namespace Grove.Effects
{
  using AI;

  public class ExileTargets : Effect
  {
    //private readonly bool _targetControllerGainsLifeEqualToToughness;
    //private readonly bool _effectControllerGainsLifeEqualToToughness;    

    public ExileTargets()
    {
      //_targetControllerGainsLifeEqualToToughness = targetControllerGainsLifeEqualToToughness;
      //_effectControllerGainsLifeEqualToToughness = effectControllerGainsLifeEqualToToughness;

      SetTags(EffectTag.Exile);
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        //if (_targetControllerGainsLifeEqualToToughness)
        //{
        //  target.Card().Controller.Life += target.Card().Toughness.GetValueOrDefault();
        //}

        //if (_effectControllerGainsLifeEqualToToughness)
        //{
        //  Controller.Life += target.Card().Toughness.GetValueOrDefault();
        //}

        target.Card().Exile();
      }
    }
  }
}