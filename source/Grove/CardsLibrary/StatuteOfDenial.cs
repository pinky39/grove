namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class StatuteOfDenial : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Statute of Denial")
        .ManaCost("{2}{U}{U}")
        .Type("Instant")
        .Text("Counter target spell. If you control a blue creature, draw a card, then discard a card.")
        .FlavorText("\"Pyrotechnic activity of any sort is strictly prohibited. It is irrelevant that today is a holiday.\"")
        .Cast(p =>
        {
          p.Text = "Counter target spell.";

          p.Effect = () => new CounterTargetSpell();

          p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

          p.TargetingRule(new EffectCounterspell());
          p.TimingRule(new WhenTopSpellIsCounterable());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "If you control a blue creature, draw a card, then discard a card.";
          p.Trigger(new OnZoneChanged(from: Zone.Hand, to: Zone.Stack)
          {
            Condition = (trigger, game) => trigger.Controller.Battlefield.Creatures.Any(c => c.HasColor(CardColor.Blue))
          });
          p.Effect = () => new DrawCards(1, discardCount: 1);
        });
    }
  }
}
