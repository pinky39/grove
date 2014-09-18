namespace Grove.CardsLibrary
{
    using System.Collections.Generic;

    public class ChildOfNight : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Child of Night")
                .ManaCost("{1}{B}")
                .Type("Creature - Vampire")
                .Text("{Lifelink}{I}(Damage dealt by this creature also causes you to gain that much life.){/I}")
                .FlavorText("Sins that would be too gruesome in the light of day are made more pleasing in the dark of night.")
                .Power(2)
                .Toughness(1)
                .SimpleAbilities(Static.Lifelink);
        }
    }
}
