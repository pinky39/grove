namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;

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
        .Abilities(
          Static.Trample,
          ActivatedAbility(
            "{1}{G}: Regenerate Child of Gaea.",
            Cost<PayMana>(c => c.Amount = "{1}{G}".ParseMana()),
            Effect<Regenerate>(),
            timing: Timings.Regenerate()),
          TriggeredAbility(
            "At the beg. of your upkeep, pay {G}{G} or sacrifice Child of Gaea.",
            Trigger<OnStepStart>(t => { t.Step = Step.Upkeep; }),
            Effect<PayManaOrSacrifice>(e =>
              {
                e.Amount = "{G}{G}".ParseMana();
                e.Message = String.Format("Pay {0}'s upkeep cost?", e.Source.OwningCard);
              }),
            triggerOnlyIfOwningCardIsInPlay: true));
    }
  }
}