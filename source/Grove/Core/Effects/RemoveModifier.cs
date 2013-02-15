namespace Grove.Core.Effects
{
  using System;

  public class RemoveModifier : Effect
  {
    private readonly Type _modifierType;
    
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