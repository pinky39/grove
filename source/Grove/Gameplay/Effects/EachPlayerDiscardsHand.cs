namespace Grove.Gameplay.Effects
{
  using System;

  public class EachPlayerDiscardsHand : Effect
  {
    protected override void ResolveEffect()
    {
      Players.Active.DiscardHand();
      Players.Passive.DiscardHand();
    }
  }
}