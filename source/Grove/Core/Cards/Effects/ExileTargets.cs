namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

  public class ExileTargets : Effect
  {
    public bool ControllerGainsLifeEqualToToughness { get; set; }    

    protected override void ResolveEffect()
    {
      foreach (var target in ValidTargets)
      {
        if (ControllerGainsLifeEqualToToughness)
        {
          target.Card().Controller.Life += target.Card().Toughness.GetValueOrDefault();
        }

        target.Card().Exile();
      }
    }
  }
}