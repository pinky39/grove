namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Triggers;

  public class Masticore : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Masticore")
        .ManaCost("{4}")
        .Type("Artifact Creature Masticore")
        .Text(
          "At the beginning of your upkeep, sacrifice Masticore unless you discard a card.{EOL}{2}: Masticore deals 1 damage to target creature.{EOL}{2}: Regenerate Masticore.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, sacrifice Masticore unless you discard a card.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new DiscardCardOrSacrificeOwner();
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Masticore deals 1 damage to target creature.";
            p.Cost = new PayMana(2.Colorless(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new DealDamageToTargets(1);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(p1 => p1.MaxRepetitions));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
            p.RepetitionRule(new RepeatForEachLifepointTargetHasLeft());
          })
        .Regenerate(cost: 2.Colorless(), text: "{2}: Regenerate Masticore.");
    }
  }
}