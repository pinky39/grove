namespace Grove.Core.Details.Cards.Effects
{
  public class TargetPlayerDrawsCards : Effect
  {
    public int CardCount { get; set; }
    public int LifeLoss { get; set; }

    public override bool NeedsTargets { get { return true; } }

    protected override void ResolveEffect()
    {
      var player = (Player) Target();
      player.DrawCards(CardCount);
      player.Life -= LifeLoss;
    }
  }
}