namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class ExileOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Exile();
    }
  }
}