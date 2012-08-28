namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class FiresOfYavimaya : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Fires of Yavimaya")
        .ManaCost("{1}{R}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have haste.{EOL}Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddStaticAbility>(
                (m, _) => m.StaticAbility = Static.Haste);
              e.CardFilter = (card, source) => card.Controller == source.Controller && card.Is().Creature;
            }),
          C.ActivatedAbility(
            "Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.",
            C.Cost<SacrificeOwner>(),
            C.Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }, untilEndOfTurn: true))),
            C.Validator(Validators.Creature()),
            targetSelectorAi: TargetSelectorAi.IncreasePowerAndToughness(2, 2),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.ToughnessIncrease
            )
        );
    }
  }
}