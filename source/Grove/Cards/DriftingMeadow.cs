namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;

  public class DriftingMeadow : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Drifting Meadow")
        .Type("Land")
        .Text(
          "Drifting Meadow enters the battlefield tapped.{EOL}{T}: Add {W} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p => p.Effect = Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true))
        .Abilities(
          ManaAbility(new ManaUnit(ManaColors.White), "{T}: Add {W} to your mana pool."));
    }
  }
}