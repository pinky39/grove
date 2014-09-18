namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using Effects;
    using Triggers;

    public class SiegeDragon : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Siege Dragon")
                .ManaCost("{5}{R}{R}")
                .Type("Creature — Dragon")
                .Text("{Flying}{EOL}When Siege Dragon enters the battlefield, destroy all Walls your opponents control.{EOL}Whenever Siege Dragon attacks, if defending player controls no Walls, it deals 2 damage to each creature without flying that player controls.")
                .Power(5)
                .Toughness(5)
                .SimpleAbilities(Static.Flying)
                .TriggeredAbility(p =>
                {
                    p.Text = "When Siege Dragon enters the battlefield, destroy all Walls your opponents control.";
                    p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

                    p.Effect = () => new DestroyAllPermanents(
                        (e, c) => c.Is().OfType("Wall") && c.Controller != e.Source.OwningCard.Controller);
                })
                .TriggeredAbility(p =>
                {
                    p.Text = "Whenever Siege Dragon attacks, if defending player controls no Walls, it deals 2 damage to each creature without flying that player controls.";

                    p.Trigger(new OnAttack()
                    {
                        Condition = (t, g) => !(t.Controller.Opponent.Battlefield.Creatures.Any(c => c.Is().OfType("Wall")))
                    });
                    
                    p.Effect = () => new DealDamageToCreaturesAndPlayers(
                        amountCreature: 2,
                        filterCreature: (e, card) => !card.Has().Flying && e.Source.OwningCard.Controller != card.Controller);
                });
        }
    }
}
