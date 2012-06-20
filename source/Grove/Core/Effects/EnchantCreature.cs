namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Modifiers;

  public class EnchantCreature : Effect
  {
    private readonly List<IModifierFactory> _modifierFactories = new List<IModifierFactory>();

    public void Modifiers(params IModifierFactory[] modifierFactories)
    {
      _modifierFactories.AddRange(modifierFactories);
    }

    protected override void ResolveEffect()
    {            
      Target.Card().Attach(
        Source.OwningCard, 
        _modifierFactories.CreateModifiers(
          Source.OwningCard, 
          Target.Card(), 
          X)        
        );
      
      Target.Card().Controller.PutCardIntoPlay(Source.OwningCard);
    }
  }
}