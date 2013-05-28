namespace Grove.Gameplay.Effects
{
  using System;

  [Serializable]
  public class RemoveModifier : Effect
  {
    private readonly Type _modifierType;

    private RemoveModifier() {}

    public RemoveModifier(Type modifierType)
    {
      _modifierType = modifierType;
    }

    protected override void ResolveEffect()
    {
      Source.OwningCard.RemoveModifier(_modifierType);
    }
  }
}