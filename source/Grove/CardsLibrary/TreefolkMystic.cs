namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Events;
  using Grove.Triggers;

  public class TreefolkMystic : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Treefolk Mystic")
        .ManaCost("{3}{G}")
        .Type("Creature Treefolk")
        .Text(
          "Whenever Treefolk Mystic blocks or becomes blocked by a creature, destroy all Auras attached to that creature.")
        .FlavorText(
          "Urza's wards fell from him like autumn leaves as he entered the dreaming grove. He awoke imprisoned in living wood.")
        .Power(2)
        .Toughness(4)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Treefolk Mystic blocks or becomes blocked by a creature, destroy all Auras attached to that creature.";
            
            p.Trigger(new WhenThisBlocks());
            p.Trigger(new WhenThisBecomesBlocked(triggerForEveryBlocker: true));            
            
            p.Effect = () => new DestroyAttachedAuras(P((e =>
              {
                var message = e.TriggerMessage<BlockerJoinedCombatEvent>();

                return message.Attacker.Card == e.Source.OwningCard
                  ? message.Blocker.Card
                  : message.Attacker.Card;
              })));
          });
    }
  }
}