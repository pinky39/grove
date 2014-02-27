namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;

  public class ThrunTheLastTroll : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thrun, the Last Troll")
        .ManaCost("{2}{G}{G}")
        .Type("Legendary Creature - Troll Shaman")
        .Text(
          "Thrun can't be countered.{EOL}Thrun can't be the target of spells or abilities your opponents control.{EOL}{1}{G}: Regenerate Thrun.")
        .FlavorText("His crime was silence, and now he suffers it eternally.")
        .Power(4)
        .Toughness(4)
        .Cast(p => p.Effect = () => new PutIntoPlay {CanBeCountered = false})
        .SimpleAbilities(Static.Hexproof)
        .Regenerate(cost: "{1}{G}".Parse(), text: "{1}{G}: Regenerate Thrun.");
    }
  }
}