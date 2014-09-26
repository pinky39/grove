namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Triggers;

    public class ReclamationSage : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Reclamation Sage")
              .ManaCost("{2}{G}")
              .Type("Creature - Elf Shaman")
              .Text("When Reclamation Sage enters the battlefield, you may destroy target artifact or enchantment.")
              .FlavorText("\"What was once formed by masons, shaped by smiths, or given life by mages, I will return to the embrace of the earth.\"")
              .Power(2)
              .Toughness(1)
              .TriggeredAbility(p =>
              {
                  p.Text = "When Reclamation Sage enters the battlefield, you may destroy target artifact or enchantment.";

                  p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                  p.Effect = () => new DestroyTargetPermanents();

                  p.TargetSelector.AddEffect(trg =>
                  {
                      trg.Is.Card(card => card.Is().Artifact || card.Is().Enchantment).On.Battlefield();
                      trg.Message = "Select an artifact or enchantment.";
                  });

                  p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Artifact || c.Is().Enchantment));
                  p.TargetingRule(new EffectDestroy());
//                  p.TargetingRule(new EffectOrCostRankBy(c => -c.Score, ControlledBy.Opponent));
              });
        }
    }
}
