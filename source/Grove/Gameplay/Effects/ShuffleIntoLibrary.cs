namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class ShuffleIntoLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.ShuffleIntoLibrary();
    }
  }
}