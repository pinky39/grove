namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class ChildOfGaea : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Child of Gaea")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature Elemental")
        .Text(
          "{Trample}{EOL}At the beginning of your upkeep, sacrifice Child of Gaea unless you pay {G}{G}.{EOL}{1}{G}: Regenerate Child of Gaea.")
        .Power(7)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Trample,
          C.ActivatedAbility(
            "{1}{G}: Regenerate Child of Gaea.",
            C.Cost<TapOwnerPayMana>((c, _) => c.Amount = "{1}{G}".ParseManaAmount()),
            C.Effect<Regenerate>(),
            timing: Timings.Regenerate()),
          C.TriggeredAbility(
            "At the beginning of your upkeep, sacrifice Child of Gaea unless you pay {G}{G}.",
            C.Trigger<AtBegginingOfStep>(t => { t.Step = Step.Upkeep; }),
            C.Effect<PayManaOrSacrifice>(e =>
              {
                e.Amount = "{G}{G}".ParseManaAmount();
                e.Message = String.Format("Pay {0}'s upkeep cost?", e.Source.OwningCard);
              }),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}