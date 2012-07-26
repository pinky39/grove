namespace Grove.Core.Details.Cards.Effects
{  
  public class ChangeController : Effect
  {
    protected override void ResolveEffect()
    {                        
      var opponent = Game.Players.GetOpponent(Controller);

      Source.OwningCard.ChangeController((Player)opponent);      
    }
  }
}