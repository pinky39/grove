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
  using Core.Mana;
  using Core.Targeting;

  public class DranaKalastriaBloodchief : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
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
          ActivatedAbility(
            "{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.",
            Cost<TapOwnerPayMana>(cost =>
              {
                cost.Amount = "{B}{B}".ParseManaAmount();
                cost.HasX = true;
                cost.XCalculator = VariableCost.TargetLifepointsLeft(ManaUsage.Abilities);
              }),
            Effect<ApplyModifiersToSelfAndToTargets>(e =>
              {
                e.ToughnessReductionTargets = Value.PlusX;

                e.SelfModifiers(
                  Modifier<AddPowerAndToughness>(m =>
                    m.Power = Value.PlusX,
                    untilEndOfTurn: true));

                e.TargetModifiers(
                  Modifier<AddPowerAndToughness>(
                    m => m.Toughness = Value.MinusX,
                    untilEndOfTurn: true)
                  );
              }),
            TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()),
            targetSelectorAi: TargetSelectorAi.ReduceToughness(),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}