namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class UntamedKavu : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Untamed Kavu")
        .ManaCost("{1}{G}")
        .Type("Creature Kavu")
        .Text("{Kicker} {3}{EOL}{Vigilance}, {trample}{EOL}If Untamed Kavu was kicked, it enters the battlefield with three +1/+1 counters on it.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Trample, Static.Vigilance)
        .Cast(p => p.Effect = () => new CastPermanent())
        .Cast(p =>

        {
          p.Cost = new PayMana("{4}{G}".Parse());
          p.Text = p.KickerDescription;
          
          p.Effect = () => new CompoundEffect(
            new CastPermanent(),
            new ApplyModifiersToSelf(              
              () =>
              {
                var tp = new TriggeredAbility.Parameters()
                {
                  Text = "If Untamed Kavu was kicked, it enters the battlefield with three +1/+1 counters on it.",
                  Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 3))
                };

                tp.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                return new AddTriggeredAbility(new TriggeredAbility(tp));
              }
            ));
        });
    }
  }
}