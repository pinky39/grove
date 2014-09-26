namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using Modifiers;

    public class SeraphOfTheMasses : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Seraph of the Masses")
                .ManaCost("{5}{W}{W}")
                .Type("Creature — Angel")
                .Text("{Convoke}{I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}{Flying}{EOL}Seraph of the Masses's power and toughness are each equal to the number of creatures you control.")
                .Power(0)
                .Toughness(0)
                .Convoke()
                .SimpleAbilities(Static.Flying)
                .StaticAbility(p =>
                {
                    p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
                      power: 1,
                      toughness: 1,
                      filter: (c, _) => c.Is().Creature,
                      modifier: () => new IntegerSetter()));
                    p.EnabledInAllZones = true;
                });
        }
    }
}
