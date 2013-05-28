namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class SacrificeOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Sacrifice();
    }
  }
}