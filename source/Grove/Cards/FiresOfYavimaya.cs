namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class FiresOfYavimaya : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Fires of Yavimaya")
        .ManaCost("{1}{R}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have haste.{EOL}Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.")
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.ModifierFactory = Modifier<AddStaticAbility>(
                m => m.StaticAbility = Static.Haste);
              e.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
            }),
          ActivatedAbility(
            "Sacrifice Fires of Yavimaya: Target creature gets +2/+2 until end of turn.",
            Cost<SacrificeOwner>(),
            Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 2;
                  m.Toughness = 2;
                }, untilEndOfTurn: true))),
            Validator(Validators.Creature()),
            selectorAi: TargetSelectorAi.IncreasePowerAndToughness(2, 2),
            timing: Timings.NoRestrictions(),
            category: EffectCategories.ToughnessIncrease
            )
        );
    }
  }
}