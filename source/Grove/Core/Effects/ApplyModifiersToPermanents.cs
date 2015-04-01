namespace Grove.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class ApplyModifiersToPermanents : Effect
  {    
    private readonly List<CardModifierFactory> _modifiers = new List<CardModifierFactory>();
    private readonly CardSelector _selector;

    private ApplyModifiersToPermanents() {}    

    public ApplyModifiersToPermanents(
      CardSelector selector,
      CardModifierFactory modifier)
    {
      _selector = selector ?? delegate { return true; };
      _modifiers.Add(modifier);
    }

    public ApplyModifiersToPermanents(
      CardSelector selector,      
      params CardModifierFactory[] modifiers)
    {      
      _selector = selector ?? delegate { return true; };
      _modifiers.AddRange(modifiers);
    }


    public override int CalculateToughnessReduction(Card card)
    {
      if ((Target == null || card.Controller == Target) && _selector(card, Ctx))
      {
        return ToughnessReduction.GetValue(X);
      }

      return 0;
    }

    protected override void ResolveEffect()
    {     
      foreach (var permanent in Players.Permanents())
      {
        if (!_selector(permanent, Ctx))
          continue;

        foreach (var modifierFactory in _modifiers)
        {
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              X = X
            };

          var modifier = modifierFactory();
          permanent.AddModifier(modifier, p);
        }
      }
    }
  }
}