namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class MilitaryIntelligence : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Military Intelligence")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text("Whenever you attack with two or more creatures, draw a card.")
        .FlavorText("To know the battlefield is to anticipate the enemy. To know the enemy is to anticipate victory.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever you attack with two or more creatures, draw a card.";

          p.Trigger(new OnAttack(triggerForEveryCreature: true)
          {
            Condition = (trigger, game) => trigger.Controller.IsActive && trigger.Controller.Battlefield.Attackers.Count() == 2
          });

          p.Effect = () => new DrawCards(1);

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
