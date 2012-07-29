namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class SmolderingCrater : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Smoldering Crater")
        .Type("Land")
        .Text(
          "Smoldering Crater enters the battlefield tapped.{EOL}{T}: Add {R} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Timing(Timings.Lands())
        .Cycling("{2}")
        .Abilities(
          C.ManaAbility(new ManaUnit(ManaColors.Red), "{T}: Add {R} to your mana pool."))
        .Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true);
    }
  }
}