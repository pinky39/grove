namespace Grove.Gameplay.Effects
{
  using System;
  using Modifiers;
  using Targeting;

  [Serializable]
  public class TargetPlayerDrawsCards : Effect
  {
    private readonly Value _cardCount;
    private readonly int _lifeLoss;

    private TargetPlayerDrawsCards() {}

    public TargetPlayerDrawsCards(Value cardCount, int lifeLoss = 0)
    {
      _cardCount = cardCount;
      _lifeLoss = lifeLoss;
    }

    protected override void ResolveEffect()
    {
      var player = Target.Player();
      player.DrawCards(_cardCount.GetValue(X));
      player.Life -= _lifeLoss;
    }
  }
}