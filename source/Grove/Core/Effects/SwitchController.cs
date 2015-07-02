namespace Grove.Effects
{
  using Modifiers;

  public class SwitchController : Effect
  {
    private Player _opponent;

    protected override void Initialize()
    {
      _opponent = Controller.Opponent;
    }
        
    protected override void ResolveEffect()
    {
      if (Source.OwningCard.Zone != Zone.Battlefield)
        return;

      if (Source.OwningCard.Controller == _opponent)
        return;
      
      var sourceModifier = new ChangeController(_opponent);      

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,          
          X = X,
          IsStatic = true,
        };
      
      Source.OwningCard.AddModifier(sourceModifier, p);
    }
  }
}