namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class AttachEquipment : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public void Modifiers(params IModifierFactory[] modifierFactories)
    {
      _modifierFactories.AddRange(modifierFactories);
    }

    public override void Resolve()
    {
      var cardToEquip = Target.Card();
      var modifiers = _modifierFactories.CreateModifiers(Source.OwningCard, Target.Card(), X);
      cardToEquip.Attach(Source.OwningCard, modifiers);
    }
  }
}