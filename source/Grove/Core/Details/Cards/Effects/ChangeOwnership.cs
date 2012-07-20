namespace Grove.Core.Details.Cards.Effects
{  
  public class ChangeOwnership : Effect
  {
    protected override void ResolveEffect()
    {                        
      var opponent = Game.Players.GetOpponent(Controller);
      opponent.GainControl(Source.OwningCard);
    }
  }
}