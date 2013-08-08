namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class SanguineGuard : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanguine Guard")
        .ManaCost("{1}{B}")
        .Type("Creature Zombie Knight")
        .Text("{First strike}{EOL}{1}{B}: Regenerate Sanguine Guard.")
        .FlavorText(
          "Father of Machines Your filigree gaze carves us, and the scars dance upon our grateful flesh.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.FirstStrike)
        .Regenerate(cost: "{1}{B}".Parse(), text: "{1}{B}: Regenerate Sanguine Guard.");
    }
  }
}