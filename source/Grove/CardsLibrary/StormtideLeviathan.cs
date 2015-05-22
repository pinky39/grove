namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Modifiers;

  public class StormtideLeviathan : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stormtide Leviathan")
        .ManaCost("{5}{U}{U}{U}")
        .Type("Creature - Leviathan")
        .Text(
          "{Islandwalk}{I}(This creature can't be blocked as long as defending player controls an Island.){/I}{EOL}All lands are Islands in addition to their other types.{EOL}Creatures without flying or islandwalk can't attack.")
        .Power(8)
        .Toughness(8)
        .SimpleAbilities(Static.Islandwalk)
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new AddStaticAbility(Static.CannotAttack);
            p.Selector = (card, effect) => (card.Is().Creature && !card.Has().Flying && !card.Has().Islandwalk);
          })
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new ChangeBasicLandSubtype("Island", replace: false);
            p.Selector = (card, ctx) => card.Is().Land;
          });
    }
  }
}