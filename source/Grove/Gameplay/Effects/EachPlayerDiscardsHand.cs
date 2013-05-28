namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class EachPlayerDiscardsHand : Effect
  {
    protected override void ResolveEffect()
    {
      Players.Active.DiscardHand();
      Players.Passive.DiscardHand();
    }
  }
}