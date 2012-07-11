namespace Grove.Core.Details.Cards.Effects
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