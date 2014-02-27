namespace Grove.Effects
{
  using Modifiers;

  public class SwitchController : Effect
  {
    protected override void ResolveEffect()
    {
      var sourceModifier = new ChangeController(Controller.Opponent);
      
      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,          
          X = X
        };
      
      Source.OwningCard.AddModifier(sourceModifier, p);
    }
  }
}