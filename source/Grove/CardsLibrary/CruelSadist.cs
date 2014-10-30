namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class CruelSadist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Cruel Sadist")
          .ManaCost("{B}")
          .Type("Creature — Human Assassin")
          .Text("{B},{T}, Pay 1 life: Put a +1/+1 counter on Cruel Sadist.{EOL}{2}{B},{T}, Remove X +1/+1 counters from Cruel Sadist: Cruel Sadist deals X damage to target creature.")
          .FlavorText("Face of innocence. Hand of death.")
          .Power(1)
          .Toughness(1)
          .ActivatedAbility(p =>
          {
            p.Text = "{B},{T}, Pay 1 life: Put a +1/+1 counter on Cruel Sadist.";
            
            p.Cost = new AggregateCost(
              new PayMana(Mana.Black, ManaUsage.Abilities),
              new Tap(),
              new PayLife(1));

            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1)).SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

            p.TimingRule(new Any(new PumpOwningCardTimingRule(1, 1), new OnEndOfOpponentsTurn()));
          })
          .ActivatedAbility(p =>
          {
            p.Text = "{2}{B},{T}, Remove X +1/+1 counters from Cruel Sadist: Cruel Sadist deals X damage to target creature.";

            p.Cost = new AggregateCost(
              new PayMana("{2}{B}".Parse(), ManaUsage.Abilities),
              new Tap(),
              new RemoveCounters(CounterType.PowerToughness));

            p.Effect = () => new DealDamageToTargets(P(e => e.Source.OwningCard.CountersCount(CounterType.PowerToughness)));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectDealDamage(p1 => p1.Card.CountersCount(CounterType.PowerToughness)));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}
