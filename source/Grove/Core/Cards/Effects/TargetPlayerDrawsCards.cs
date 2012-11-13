namespace Grove.Core.Cards.Effects
{
  public class TargetPlayerDrawsCards : Effect
  {
    public int CardCount { get; set; }
    public int LifeLoss { get; set; }    

    protected override void ResolveEffect()
    {
      var player = (Player) Target();
      player.DrawCards(CardCount);
      player.Life -= LifeLoss;
    }
  }
}