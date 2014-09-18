namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;
    using Triggers;

    public class Meteorite : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Meteorite")
              .ManaCost("{5}")
              .Type("Artifact")
              .Text("When Meteorite enters the battlefield, it deals 2 damage to target creature or player.{EOL}{T}: Add one mana of any color to your mana pool.")
              .FlavorText("\"And if I'm lying,\" he began...")
              .TriggeredAbility(p =>
              {
                  p.Text = "When Meteorite enters the battlefield, it deals 2 damage to target creature or player";

                  p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                  p.Effect = () => new DealDamageToTargets(2);
                  p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

                  p.TargetingRule(new EffectDealDamage(2));
              })
              .ManaAbility(p =>
              {
                  p.Text = "{T}: Add one mana of any color to your mana pool.";
                  p.ManaAmount(Mana.Any);
              });
        }
    }
}
