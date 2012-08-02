namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;

  public class SwitchController : Effect
  {
    protected override void ResolveEffect()
    {
      var opponent = Game.Players.GetOpponent(Controller);

      var modifier = new Modifier.Factory<ChangeController>
        {
          Game = Game,
          Init = (m, c) => m.NewController = opponent
        }
        .CreateModifier(Source.OwningCard, Source.OwningCard);
      
      
      Source.OwningCard.AddModifier(modifier);            
    }
  }
}