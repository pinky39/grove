namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;

  public class VinesOfVastwood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Vines of Vastwood")
        .ManaCost("{G}")
        .KickerCost("{G}")
        .Type("Instant")
        .Category(EffectCategories.Protector | EffectCategories.ToughnessIncrease)
        .Timing(Timings.NoRestrictions())
        .Text(
          "{Kicker} {G}{EOL}Target creature can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that creature gets +4/+4 until end of turn.")
        .Effect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
          c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true)))
        .Targets(
          filter: TargetFilters.ShieldHexproof(),
          selectors: C.Selector(Selectors.Creature()))
        .KickerEffect<ApplyModifiersToTarget>((e, c) => e.Modifiers(
          c.Modifier<AddStaticAbility>((m, _) => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true),
          c.Modifier<AddPowerAndToughness>((m, _) =>
            {
              m.Power = 4;
              m.Toughness = 4;
            }, untilEndOfTurn: true)))
        .KickerTargets(
          filter: Any(TargetFilters.ShieldHexproof(), TargetFilters.IncreasePowerAndToughness(4, 4)),
          selectors: C.Selector(Selectors.Creature()));
    }
  }
}