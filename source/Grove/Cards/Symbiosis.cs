namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class Symbiosis : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Symbiosis")
        .ManaCost("{1}{G}")
        .Type("Instant")
        .Category(EffectCategories.ToughnessIncrease)
        .Timing(Timings.NoRestrictions())
        .Text("Two target creatures each get +2/+2 until end of turn.")
        .FlavorText(
          "Although the elves of Argoth always considered them a nuisance, the pixies made fine allies during the war against the machines.")
        .Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }, untilEndOfTurn: true)))
        .Targets(
          selectorAi: TargetSelectorAi.IncreasePowerAndToughness(2, 2),
          effectValidator: Validator(Validators.Creature(), minCount: 2, maxCount: 2)
        );
    }
  }
}