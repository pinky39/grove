namespace Grove.Gameplay.Effects
{
  using System;
  using Targeting;

  [Serializable]
  public class UntapTargetPermanents : Effect
  {
    protected override void ResolveEffect()
    {
      foreach (var validTarget in ValidEffectTargets)
      {
        validTarget.Card().Untap();
      }
    }
  }
}