namespace Grove.Core.Cards.Effects
{
  using System.Linq;
  using Modifiers;

  public class GainControlOfOwnedPermanents : Effect
  {
    protected override void ResolveEffect()
    {
      var opponent = Game.Players.GetOpponent(Controller);

      foreach (var permanent in opponent.Battlefield.ToList())
      {
        if (permanent.Owner == Controller)
        {
          var modifier = Builder
            .Modifier<ChangeController>(m => m.NewController = Controller)
            .CreateModifier(Source.OwningCard, permanent, X, Game);

          permanent.AddModifier(modifier);
        }
      }
    }
  }
}