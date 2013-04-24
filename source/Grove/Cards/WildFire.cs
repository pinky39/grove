namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class WildFire : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wildfire")
        .ManaCost("{4}{R}{R}")
        .Type("Sorcery")
        .Text("Each player sacrifices four lands. Wildfire deals 4 damage to each creature.")
        .FlavorText("'Shiv hatched from a shell of stone around a yolk of flame.'—Viashino myth")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new PlayersSacrificeLands(4),
              new DealDamageToCreaturesAndPlayers(amountCreature: 4));
          });
    }
  }
}