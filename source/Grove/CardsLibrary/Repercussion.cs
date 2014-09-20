namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Events;
  using Triggers;

  public class Repercussion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Repercussion")
        .ManaCost("{1}{R}{R}")
        .Type("Enchantment")
        .Text("Whenever a creature is dealt damage, Repercussion deals that much damage to that creature's controller.")
        .FlavorText("Not all Keld's warriors are found on the battlefield.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever a creature is dealt damage, Repercussion deals that much damage to that creature's controller.";

            p.Trigger(new OnDamageDealt(
              creatureFilter: delegate { return true; },
              onlyByTriggerSource: false));

            p.Effect = () => new DealExistingDamageToPlayer(
              P(e => e.TriggerMessage<DamageDealtEvent>().Damage),
              P(e =>
                {
                  var creature = (Card) e.TriggerMessage<DamageDealtEvent>().Receiver;
                  return creature.Controller;
                }));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}