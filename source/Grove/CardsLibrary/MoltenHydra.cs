namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.RepetitionRules;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class MoltenHydra : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Molten Hydra")
        .ManaCost("{1}{R}")
        .Type("Creature Hydra")
        .Text(
          "{1}{R}{R}: Put a +1/+1 counter on Molten Hydra.{EOL}{T},Remove all +1/+1 counters from Molten Hydra: Molten Hydra deals damage to target creature or player equal to the number of +1/+1 counters removed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{R}{R}: Put a +1/+1 counter on Molten Hydra.";
            p.Cost = new PayMana("{1}{R}{R}".Parse(), ManaUsage.Abilities, supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);
            
            p.TimingRule(new Any(new PumpOwningCardTimingRule(1, 1), new OnEndOfOpponentsTurn()));
            p.RepetitionRule(new RepeatMaxTimes());
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T},Remove all +1/+1 counters from Molten Hydra: Molten Hydra deals damage to target creature or player equal to the number of +1/+1 counters removed this way.";

            p.Cost = new AggregateCost(
              new Tap(),
              new RemoveCounters(CounterType.PowerToughnes));

            p.Effect =
              () => new DealDamageToTargets(P(e => e.Source.OwningCard.CountersCount(CounterType.PowerToughnes)));
            
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            
            p.TimingRule(new WhenCardHas(c => c.CountersCount(CounterType.PowerToughnes) > 0));
            p.TargetingRule(new EffectDealDamage(p1 => p1.Card.CountersCount(CounterType.PowerToughnes)));            
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });

    }
  }
}