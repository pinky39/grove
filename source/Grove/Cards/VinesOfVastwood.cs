namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Targeting;

  public class VinesOfVastwood : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Vines of Vastwood")
        .ManaCost("{G}")
        .KickerCost("{G}")
        .Type("Instant")
        .Category(EffectCategories.Protector | EffectCategories.ToughnessIncrease)
        .Timing(Timings.NoRestrictions())
        .Text(
          "{Kicker} {G}{EOL}Target creature can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that creature gets +4/+4 until end of turn.")
        .Effect<Core.Cards.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true)))
        .Targets(
          TargetSelectorAi.ShieldHexproof(), 
          TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()))
        .KickerEffect<Core.Cards.Effects.ApplyModifiersToTargets>(e => e.Modifiers(
          Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Hexproof, untilEndOfTurn: true),
          Modifier<AddPowerAndToughness>(m =>
            {
              m.Power = 4;
              m.Toughness = 4;
            }, untilEndOfTurn: true)))
        .KickerTargets(
          aiTargetSelector: Any(TargetSelectorAi.ShieldHexproof(), TargetSelectorAi.IncreasePowerAndToughness(4, 4)),
          effectValidators: TargetValidator(TargetIs.Card(x => x.Is().Creature), ZoneIs.Battlefield()));
    }
  }
}