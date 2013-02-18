namespace Grove.Core.Effects
{
  using Targeting;

  public class TargetPlayerDrawsCards : Effect
  {
    private readonly int _cardCount;
    private readonly int _lifeLoss;

    private TargetPlayerDrawsCards() {}

    public TargetPlayerDrawsCards(int cardCount, int lifeLoss = 0)
    {
      _cardCount = cardCount;
      _lifeLoss = lifeLoss;
    }

    protected override void ResolveEffect()
    {
      var player = Target.Player();
      player.DrawCards(_cardCount);
      player.Life -= _lifeLoss;
    }
  }
}