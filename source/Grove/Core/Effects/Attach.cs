namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Modifiers;

  public class Attach : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public bool ModifiesAttachmentController;
    public Value ToughnessReduction = 0;

    public override int CalculateToughnessReduction(Card card)
    {
      if (Target() == card)
      {
        return ToughnessReduction.GetValue(X);
      }

      return 0;
    }

    public void Modifiers(params IModifierFactory[] modifierFactories)
    {
      _modifierFactories.AddRange(modifierFactories);
    }

    protected override void ResolveEffect()
    {
      var modifiers = _modifierFactories.CreateModifiers(
        Source.OwningCard,
        Target().Card(),
        X, 
        Game);

      Target().Card().Attach(Source.OwningCard);

      if (ModifiesAttachmentController)
      {
        foreach (var modifier in modifiers)
        {
          Controller.AddModifier(modifier);
        }
      }
      else
      {
        foreach (var modifier in modifiers)
        {
          Target().Card().AddModifier(modifier);
        }
      }
    }
  }
}