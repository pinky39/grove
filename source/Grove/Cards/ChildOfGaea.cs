namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class ChildOfGaea : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Child of Gaea")
        .ManaCost("{3}{G}{G}{G}")
        .Type("Creature Elemental")
        .Text(
          "{Trample}{EOL}At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.{EOL}{1}{G}: Regenerate Child of Gaea.")
        .Power(7)
        .Toughness(7)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Trample,
          ActivatedAbility(
            "{1}{G}: Regenerate Child of Gaea.",
            Cost<PayMana>(c => c.Amount = "{1}{G}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate()),
          TriggeredAbility(
            "At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.",
            Trigger<AtBegginingOfStep>(t => { t.Step = Step.Upkeep; }),
            Effect<PayManaOrSacrifice>(e =>
              {
                e.Amount = "{G}{G}".ParseMana();
                e.Message = String.Format("Pay {0}'s upkeep cost?", e.Source.OwningCard);
              }),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}