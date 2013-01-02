namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Counters;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;


  public class RagingRavine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Raging Ravine")
        .Type("Land")
        .Text(
          "Raging Ravine enters the battlefield tapped.{EOL}{T}: Add {R} or {G} to your mana pool.{EOL}{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.")
        .Timing(Timings.Lands())
        .Effect<PutIntoPlay>(e => e.PutIntoPlayTapped = true)
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."),
          ActivatedAbility(
            "{2}{R}{G}: Until end of turn, Raging Ravine becomes a 3/3 red and green Elemental creature with Whenever this creature attacks, put a +1/+1 counter on it. It's still a land.",
            Cost<PayMana>(cost =>
              {
                cost.Amount = "{2}{R}{G}".ParseMana();
                cost.TryNotToConsumeCardsManaSourceWhenPayingThis = true;
              }),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 3;
                  m.Colors = ManaColors.Red | ManaColors.Green;
                  m.Type = "Land Creature - Elemental";
                }, untilEndOfTurn: true),
              Modifier<AddTriggeredAbility>(m =>
                {
                  m.Ability = TriggeredAbility(
                    "Whenever this creature attacks, put a +1/+1 counter on it.",
                    Trigger<OnAttack>(),
                    Effect<ApplyModifiersToSelf>(p1 => p1.Effect.Modifiers(
                      Modifier<AddCounters>(mo => mo.Counter = Counter<PowerToughness>(counter =>
                        {
                          counter.Power = 1;
                          counter.Toughness = 1;
                        })))));
                }, untilEndOfTurn: true))),
            timing: Timings.ChangeToCreature(minAvailableMana: 5)
            ));
    }
  }
}