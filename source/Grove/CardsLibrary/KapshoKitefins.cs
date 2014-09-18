namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Triggers;

    public class KapshoKitefins : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Kapsho Kitefins")
              .ManaCost("{4}{U}{U}")
              .Type("Creature - Fish")
              .Text("{Flying}{EOL}Whenever Kapsho Kitefins or another creature enters the battlefield under your control, tap target creature an opponent controls.")
              .FlavorText("\"It's a truly disconcerting sight to see their shadows cast upon the deck.\"{EOL}—Captain Triff")
              .Power(3)
              .Toughness(3)
              .SimpleAbilities(Static.Flying)
              .TriggeredAbility(p =>
              {
                  p.Text = "Whenever Kapsho Kitefins or another creature enters the battlefield under your control, tap target creature an opponent controls.";

                  p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                  p.Trigger(new OnZoneChanged(
                      to: Zone.Battlefield,
                      filter: (c, a, g) => a.OwningCard.Controller == c.Controller && c.Is().Creature && a.OwningCard.IsPermanent));

                  p.Effect = () => new TapTargets();

                  p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.Opponent).On.Battlefield());

                  p.TargetingRule(new EffectTapCreature());
              });
        }
    }
}
