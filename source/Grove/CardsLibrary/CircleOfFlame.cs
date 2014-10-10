namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Events;
  using Triggers;

  public class CircleOfFlame : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Circle of Flame")
        .ManaCost("{1}{R}")
        .Type("Enchantment")
        .Text("Whenever a creature without flying attacks you or a planeswalker you control, Circle of Flame deals 1 damage to that creature.")
        .FlavorText("\"Which do you think is a better deterrent: a moat of water or one of fire?\"—Chandra Nalaar")
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever a creature without flying attacks you or a planeswalker you control, Circle of Flame deals 1 damage to that creature.";

          p.Trigger(new OnAttack(triggerForCreature: 
            (card, trigger) => !card.Has().Flying && card.Controller != trigger.Controller));

          p.Effect = () => new DealDamageToCreature(amount: 1, creature: P(e => e.TriggerMessage<AttackerJoinedCombatEvent>().Attacker.Card));

          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
