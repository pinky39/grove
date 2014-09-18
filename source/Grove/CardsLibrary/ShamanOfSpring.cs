namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class ShamanOfSpring : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Shaman of Spring")
              .ManaCost("{3}{G}")
              .Type("Creature - Elf Shaman")
              .Text("When Shaman of Spring enters the battlefield, draw a card.")
              .FlavorText("Some shamanic sects advocate the different seasons, each working to preserve nature's cycles.")
              .Power(2)
              .Toughness(2)
              .TriggeredAbility(p =>
              {
                  p.Text = "When Shaman of Spring enters the battlefield, draw a card.";

                  p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                  p.Effect = () => new DrawCards(1);

                  p.TimingRule(new OnFirstMain());
              });
        }
    }
}
