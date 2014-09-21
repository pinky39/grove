namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class KrenkosEnforcer : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Krenko's Enforcer")
                .ManaCost("{1}{R}{R}")
                .Type("Creature — Goblin Warrior")
                .Text("{Intimidate}{I}(This creature can't be blocked except by artifact creatures and/or creatures that share a color with it.){/I}")
                .FlavorText("He just likes to break legs. Collecting the debt is a bonus.")
                .Power(2)
                .Toughness(2)
                .SimpleAbilities(Static.Intimidate);
        }
    }
}
