namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class UntapOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Untap();
    }
  }
}