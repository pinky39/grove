namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class ExileTargetPermanent : Effect
  {
    public bool ControllerGainsLifeEqualToToughness { get; set; }

    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      if (ControllerGainsLifeEqualToToughness)
      {
        Target().Card().Controller.Life += Target().Card().Toughness.Value;
      }

      Target().Card().Exile();
    }
  }
}