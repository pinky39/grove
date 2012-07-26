namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class DranaKalastriaBloodchief : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Drana, Kalastria Bloodchief")
        .ManaCost("{3}{B}{B}")
        .Type("Legendary Creature - Vampire Shaman")
        .Text(
          "{Flying}{EOL}{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.")
        .Power(4)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying,
          C.ActivatedAbility(
            "{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, c) =>
              {
                cost.Amount = "{B}{B}".ParseManaAmount();
                cost.HasX = true;
                cost.XCalculator = VariableCost.TargetLifepointsLeft();
              }),
            C.Effect<ApplyModifiersToSelfAndToTargets>(p =>
              {
                p.Effect.ToughnessReductionTargets = Value.PlusX;

                p.Effect.SelfModifiers(
                  p.Builder.Modifier<AddPowerAndToughness>((m, _) =>
                    m.Power = Value.PlusX,
                    untilEndOfTurn: true));
                
                p.Effect.TargetModifiers(
                  p.Builder.Modifier<AddPowerAndToughness>(
                    (m, _) => m.Toughness = Value.MinusX,
                    untilEndOfTurn: true)
                  );
              }),
            C.Validator(Validators.Creature()),
            aiTargetFilter: AiTargetSelectors.ReduceToughness(),
            timing: Timings.TargetRemovalInstant()));
    }
  }
}