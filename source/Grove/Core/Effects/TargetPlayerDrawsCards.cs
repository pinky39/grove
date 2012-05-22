namespace Grove.Core.Effects
{
  public class TargetPlayerDrawsCards : Effect
  {
    public int CardCount { get; set; }
    public int LifeLoss { get; set; }

    public override void Resolve()
    {
      var player = (Player) Target;
      player.DrawCards(CardCount);
      player.Life -= LifeLoss;
    }
  }
}