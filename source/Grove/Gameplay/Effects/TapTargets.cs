namespace Grove.Gameplay.Effects
{
  using System;
  using Targeting;

  [Serializable]
  public class TapTargets : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().Tap();
      }
    }
  }
}