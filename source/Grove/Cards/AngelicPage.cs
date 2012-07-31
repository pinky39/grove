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

  public class AngelicPage : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
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
          C.ActivatedAbility(
            "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.TapOwner = true),
            C.Effect<ApplyModifiersToTargets>(p => p.Effect.Modifiers(
              p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                {
                  m.Power = 1;
                  m.Toughness = 1;
                },
                untilEndOfTurn: true))),
            C.Validator(Validators.AttackerOrBlocker()),
            targetSelectorAi: TargetSelectorAi.PumpAttackerOrBlocker(power: 1, thougness: 1),
            timing: Timings.DeclareBlockers(),
            category: EffectCategories.ToughnessIncrease));
    }
  }
}