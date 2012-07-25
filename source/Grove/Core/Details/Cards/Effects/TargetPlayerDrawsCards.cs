namespace Grove.Core.Details.Cards.Effects
{
  public class TargetPlayerDrawsCards : Effect
  {
    public int CardCount { get; set; }
    public int LifeLoss { get; set; }

    protected override void ResolveEffect()
    {
      var player = Target().Player();
      player.DrawCards(CardCount);
      player.Life -= LifeLoss;
    }
  }
}