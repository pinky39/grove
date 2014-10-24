namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class NightfireGiant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nightfire Giant")
        .ManaCost("{4}{B}")
        .Type("Creature — Zombie Giant")
        .Text(
          "Nightfire Giant gets +1/+1 as long as you control a Mountain.{EOL}{4}{R}: Nightfire Giant deals 2 damage to target creature or player.")
        .FlavorText("Nightfire turns the greatest weakness of the undead into formidable strength.")
        .Power(4)
        .Toughness(3)
        .StaticAbility(p => 
          p.Modifier(() => new AddPowerToughnessAsLongAsYouControlPermanent(1, 1, c => c.Is("mountain"))))
        .ActivatedAbility(p =>
          {
            p.Text = "{4}{R}: Nightfire Giant deals 2 damage to target creature or player.";

            p.Cost = new PayMana("{4}{R}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(p1 => 2*p1.MaxRepetitions));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
            p.RepetitionRule(new RepeatForEachLifepointTargetHasLeft());
          });
    }
  }
}