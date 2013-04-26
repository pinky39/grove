namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai;
  using Ai.TargetingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class VinesOfVastwood : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Vines of Vastwood")
        .ManaCost("{G}")
        .Type("Instant")
        .Text(
          "{Kicker} {G}{EOL}Target creature can't be the target of spells or abilities your opponents control this turn. If Vines of Vastwood was kicked, that creature gets +4/+4 until end of turn.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Hexproof) {UntilEot = true})
              {Category = EffectCategories.Protector};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new GainHexproof());
          })
        .Cast(p =>
          {
            p.Text = p.KickerDescription;
            p.Cost = new PayMana("{G}{G}".Parse(), ManaUsage.Spells);
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Hexproof) {UntilEot = true},
              () => new AddPowerAndToughness(4, 4) {UntilEot = true})
              {Category = EffectCategories.Protector | EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new IncreasePowerOrToughness(4, 4));
          });
    }
  }
}