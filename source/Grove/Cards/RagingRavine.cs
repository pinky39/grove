namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Counters;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

  public class RagingRavine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Raging Ravine")
        .Type("Land")
        .Text(
          "Raging Ravine enters the battlefield tapped.{EOL}{T}: Add {R} or {G} to your mana pool.{EOL}{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.")
        .Timing(Timings.Lands)
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = delegate { return true; })
        .Abilities(
          C.ManaAbility(
            new Mana(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."),
          C.ActivatedAbility(
            "{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.",
            C.Cost<TapOwnerPayMana>((cost, _) =>
              {
                cost.Amount = "{2}{R}{G}".ParseManaAmount();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            C.Effect<ApplyModifiersToSelf>((e, c) => e.Modifiers(
              c.Modifier<ChangeToCreature>((m, _) =>
                {
                  m.Power = 3;
                  m.Tougness = 3;
                  m.Colors = ManaColors.Red | ManaColors.Green;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              c.Modifier<AddTriggeredAbility>((m, c0) =>
                {
                  m.Ability = c0.TriggeredAbility(
                    "Whenever this creature attacks, put a +1/+1 counter on it.",
                    c0.Trigger<OnAttack>(),
                    c0.Effect<ApplyModifiersToSelf>((ef, c1) => ef.Modifiers(
                      c1.Modifier<AddCounters>((mo, c2) => mo.Counter = c2.Counter<PowerToughness>((counter, _) =>
                        {
                          counter.Power = 1;
                          counter.Toughness = 1;
                        })))));
                }, untilEndOfTurn: true))),
            timing: Timings.Steps(Step.BeginningOfCombat)));
    }
  }
}