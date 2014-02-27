namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.Triggers;

  public class KeldonChampion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Keldon Champion")
        .ManaCost("{2}{R}{R}")
        .Type("Creature Human Barbarian")
        .Text("{Echo} {2}{R}{R}, {haste}{EOL}When Keldon Champion enters the battlefield, it deals 3 damage to target player.")
        .Power(3)
        .Toughness(2)
        .SimpleAbilities(Static.Haste)
        .Echo("{2}{R}{R}")
        .TriggeredAbility(p =>
          {
            p.Text = "When Keldon Champion enters the battlefield, it deals 3 damage to target player.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());
            p.TargetingRule(new EffectOpponent());
          });
    }
  }
}