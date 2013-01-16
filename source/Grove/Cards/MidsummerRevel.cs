namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Costs;
  using Core.Counters;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Triggers;

  public class MidsummerRevel : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Midsummer Revel")
        .ManaCost("{3}{G}{G}")
        .Type("Enchantment")
        .Text(
          "At the beginning of your upkeep, you may put a verse counter on Midsummer Revel.{EOL}{G},Sacrifice Midsummer Revel: Put X 3/3 green Beast creature tokens onto the battlefield, where X is the number of verse counters on Midsummer Revel.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "At the beginning of your upkeep, you may put a verse counter on Midsummer Revel.",
            Trigger<OnStepStart>(t => t.Step = Step.Upkeep),
            Effect<ApplyModifiersToSelf>(e => e.Modifiers(
              Modifier<AddCounters>(m => { m.Counter = Counter<ChargeCounter>(); }))),
            triggerOnlyIfOwningCardIsInPlay: true
            ),
          ActivatedAbility(
            "{G},Sacrifice Midsummer Revel: Put X 3/3 green Beast creature tokens onto the battlefield, where X is the number of verse counters on Midsummer Revel.",
            Cost<PayMana, Sacrifice>(cost => cost.Amount = ManaAmount.Green),
            Effect<CreateTokens>(e =>
              {
                e.Count = e.Source.OwningCard.Counters.GetValueOrDefault();
                e.Tokens(Card
                  .Named("Beast Token")
                  .FlavorText(
                    "All we know about the Krosan Forest we have learned from those few who made it out alive.{EOL}—Elvish refugee")
                  .Power(3)
                  .Toughness(3)
                  .Type("Creature Token Beast")
                  .Colors(ManaColors.Green));
              }),
              timing: Timings.Has3CountersOr1IfDestroyed())
            );
    }
  }
}