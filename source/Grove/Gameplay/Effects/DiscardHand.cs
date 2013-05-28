namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class DiscardHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.DiscardHand();
    }
  }
}