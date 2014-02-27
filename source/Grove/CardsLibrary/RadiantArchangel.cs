namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Modifiers;

  public class RadiantArchangel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Radiant, Archangel")
        .ManaCost("{3}{W}{W}")
        .Type("Legendary Creature Angel")
        .Text(
          "{Flying}, {vigilance}{EOL}Radiant, Archangel gets +1/+1 for each other creature with flying on the battlefield.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying, Static.Vigilance)
        .StaticAbility(p =>
          {
            p.Modifier(() => new ModifyPowerToughnessForEachPermanent(
              power: 1,
              toughness: 1,
              filter: (c, m) => c.Is().Creature && c.Has().Flying && c != m.OwningCard,
              modifier: () => new IntegerIncrement(),
              controlledBy: ControlledBy.Any
              ));

            p.EnabledInAllZones = false;
          });
    }
  }
}