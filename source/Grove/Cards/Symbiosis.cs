namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
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
        .Effect<Core.Cards.Effects.ApplyModifiersToTargets>(p => p.Effect.Modifiers(
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 2;
              m.Toughness = 2;
            }, untilEndOfTurn: true)))
        .Targets(
          TargetSelectorAi.IncreasePowerAndToughness(2, 2), 
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield(), minCount: 2, maxCount: 2)
        );
    }
  }
}