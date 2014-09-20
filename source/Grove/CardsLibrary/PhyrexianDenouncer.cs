namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class PhyrexianDenouncer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Denouncer")
        .ManaCost("{1}{B}")
        .Type("Creature Carrier")
        .Text("{T}, Sacrifice Phyrexian Denouncer: Target creature gets -1/-1 until end of turn.")
        .FlavorText("The first stage of the illness: rash and nausea.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}, Sacrifice Phyrexian Denouncer: Target creature gets -1/-1 until end of turn.";
            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-1, -1) { UntilEot = true }) { ToughnessReduction = 1 };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness(1));
            p.TimingRule(new Any(new WhenOwningCardWillBeDestroyed(), new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness, combatOnly: true)));
          });
    }
  }
}