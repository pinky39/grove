namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class Attach : Effect
  {
    private readonly List<ModifierFactory> _modifiers = new List<ModifierFactory>();
    private readonly bool _modifiesAttachmentController;

    private Attach() {}

    public Attach(params ModifierFactory[] modifiers) : this(false, modifiers) {}

    public Attach(bool modifiesAttachmentController, params ModifierFactory[] modifiers)
    {
      _modifiers.AddRange(modifiers);
      _modifiesAttachmentController = modifiesAttachmentController;
    }

    public override int CalculateToughnessReduction(Card card)
    {
      if (Target == card)
      {
        return ToughnessReduction.GetValue(X);
      }
      return 0;
    }

    protected override void ResolveEffect()
    {
      Target.Card().Attach(Source.OwningCard);

      foreach (var modifierFactory in _modifiers)
      {
        var p = new ModifierParameters
          {
            SourceEffect = this,
            SourceCard = Source.OwningCard,
            Target = Target,
            X = X
          };

        var modifier = modifierFactory().Initialize(p, Game);
        if (_modifiesAttachmentController)
        {
          Controller.AddModifier(modifier);
        }
        else
        {
          Target.AddModifier(modifier);
        }
      }
    }
  }
}