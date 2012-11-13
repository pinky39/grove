namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class DriftingDjinn : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Drifting Djinn")
        .ManaCost("{4}{U}{U}")
        .Type("Creature - Djinn")
        .Text(
          "{Flying}{EOL}At the beg. of your upkeep, sacrifice Drifting Djinn unless you pay {1}{U}.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Power(5)
        .Toughness(5)
        .Timing(Timings.Creatures())
        .Cycling("{2}")
        .Abilities(
          Static.Flying,
          TriggeredAbility(
            "At the beginning of your upkeep, sacrifice Drifting Djinn unless you pay {1}{U}.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.Upkeep; }),
            Effect<PayManaOrSacrifice>(e =>
              {
                e.Amount = "{1}{U}".ParseManaAmount();
                e.Message = String.Format("Pay {0}'s upkeep cost?", e.Source.OwningCard);
              }),
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}