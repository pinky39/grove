namespace Grove.Gameplay.Effects
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
          var p = new ModifierParameters
            {
              SourceEffect = this,
              SourceCard = Source.OwningCard,
              Target = permanent,
              X = X
            };

          var modifier = new ChangeController(Controller)
            .Initialize(p, Game);

          permanent.AddModifier(modifier);
        }
      }
    }
  }
}