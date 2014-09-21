using System.Collections.Generic;

namespace Grove.CardsLibrary
{
    class AccursedSpirit : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
              .Named("Accursed Spirit")
              .ManaCost("{3}{B}")
              .Type("Creature - Spirit")
              .Text("{Intimidate}{I}(This creature can't be blocked except by artifact creatures and/or creatures that share a color with it.){/I}")
              .FlavorText("Many have heard the slither of dragging armor and the soft squelch of its voice. But only its victims ever meet its icy gaze.")
              .Power(3)
              .Toughness(2)
              .SimpleAbilities(Static.Intimidate);
        }
    }
}
