namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class AttachEquipment : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public void Modifiers(params IModifierFactory[] modifierFactories)
    {
      _modifierFactories.AddRange(modifierFactories);
    }

    public override bool NeedsTargets
    {
      get { return true; }
    }

    protected override void ResolveEffect()
    {
      var cardToEquip = Target().Card();

      var modifiers = _modifierFactories.CreateModifiers(
        Source.OwningCard,
        Target().Card(),
        this,
        X);

      cardToEquip.Attach(Source.OwningCard);

      foreach (var modifier in modifiers)
      {
        cardToEquip.AddModifier(modifier);
      }
    }
  }
}