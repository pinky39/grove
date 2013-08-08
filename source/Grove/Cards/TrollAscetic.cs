namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class TrollAscetic : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Troll Ascetic")
        .ManaCost("{1}{G}{G}")
        .Type("Creature - Troll Shaman")
        .Text(
          "{Hexproof}{EOL}{1}{G}: Regenerate Troll Ascetic.")
        .FlavorText("It's no coincidence that the oldest trolls are also the angriest.")
        .Power(3)
        .Toughness(2)
        .SimpleAbilities(Static.Hexproof)
        .Regenerate(cost: "{1}{G}".Parse(), text: "{1}{G}: Regenerate Troll Ascetic.");
    }
  }
}