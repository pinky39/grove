namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;
  using Core.Modifiers;

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
        .Abilities(
          StaticAbility.Flying,
          C.ActivatedAbility(
            "{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.",
            C.Cost<TapOwnerPayMana>((cost, c) =>
            {
              cost.Amount = "{B}{B}".ParseManaAmount();
              cost.HasX = true;
              cost.XCalculator = VariableCost.TargetLifepointsLeft();
            }),
            C.Effect<ApplyModifiersToSelfAndToTargets>((e, c) =>
            {
              e.SelfModifiers(
                c.Modifier<AddPowerAndToughness>((m, _) =>
                  m.Power = Value.PlusX,
                  untilEndOfTurn: true));
              e.TargetModifiers(
                c.Modifier<AddPowerAndToughness>(
                  (m, _) => m.Toughness = Value.MinusX,
                  untilEndOfTurn: true)
                );
            }),
            C.Selector(
              target => target.Is().Creature,
              Core.Ai.TargetScores.OpponentStuffScoresMore()
              ),
            timing: Timings.InstantRemoval(),
            category: EffectCategories.ToughnessReduction));
    }
  }
}