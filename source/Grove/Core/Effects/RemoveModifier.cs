namespace Grove.Core.Effects
{
  using System;

  public class RemoveModifier : Effect
  {
    public Type ModifierType;
    
    protected override void ResolveEffect()
    {
      Source.OwningCard.RemoveModifier(ModifierType);
    }
  }
}