namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class EnchantCreature : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public bool ModifiesEnchantmentController;

    public void Modifiers(params IModifierFactory[] modifierFactories)
    {
      _modifierFactories.AddRange(modifierFactories);
    }

    protected override void ResolveEffect()
    {
      var modifiers = _modifierFactories.CreateModifiers(
        Source.OwningCard,
        Target().Card(),
        X);
            
      Target().Card().Attach(Source.OwningCard);
      
      if (ModifiesEnchantmentController)
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

      Target().Card().Controller.PutCardIntoPlay(Source.OwningCard);
    }
  }
}