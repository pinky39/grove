namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class GoblinBerserker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Berserker")
        .ManaCost("{3}{R}")
        .Type("Creature Goblin Berserker")
        .Text("{First strike}, {haste}")
        .FlavorText("Goblins don't know the meaning of the word 'tactics'—or the word 'meaning,' for that matter.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Haste, Static.FirstStrike);
    }
  }
}