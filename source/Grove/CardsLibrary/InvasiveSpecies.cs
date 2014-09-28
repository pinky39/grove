namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class InvasiveSpecies : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Invasive Species")
              .ManaCost("{2}{G}")
              .Type("Creature - Insect")
              .Text("When Invasive Species enters the battlefield, return another permanent you control to its owner's hand.")
              .FlavorText("It's easier to relocate a village that lies in their path than to turn the bugs aside.")
              .Power(3)
              .Toughness(3)
              .TriggeredAbility(p =>
              {
                  p.Text = "When Invasive Species enters the battlefield, return another permanent you control to its owner's hand.";

                  p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
                  
                  p.Effect = () => new ReturnToHand();
                  
                  p.TargetSelector.AddEffect(trg => trg.Is.Card(controlledBy: ControlledBy.SpellOwner, canTargetSelf: false).On.Battlefield());

                  p.TargetingRule(new EffectBounce());
                  p.TimingRule(new OnSecondMain());
              });
        }
    }
}
