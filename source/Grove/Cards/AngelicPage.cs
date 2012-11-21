namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class AngelicPage : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Page")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +1/+1 until end of turn.")
        .FlavorText("If only every message were as perfect as its bearers.")
        .Timing(Timings.Creatures())
        .Power(1)
        .Toughness(1)
        .Abilities(
          Static.Flying,
          ActivatedAbility(
            "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.",
            Cost<TapOwnerPayMana>(cost => cost.TapOwner = true),
            Effect<ApplyModifiersToTargets>(e => e.Modifiers(
              Modifier<AddPowerAndToughness>(m =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                },
                untilEndOfTurn: true))),
            TargetValidator(TargetIs.AttackerOrBlocker(), ZoneIs.Battlefield()),
            targetSelectorAi: TargetSelectorAi.PumpAttackerOrBlocker(power: 1, thougness: 1),
            timing: Timings.DeclareBlockers(),
            category: EffectCategories.ToughnessIncrease));
    }
  }
}