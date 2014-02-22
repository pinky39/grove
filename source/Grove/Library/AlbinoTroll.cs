namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;

  public class AlbinoTroll : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Albino Troll")
        .ManaCost("{1}{G}")
        .Type("Creature Troll")
        .Text(
          "{Echo} {1}{G} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.){EOL}{1}{G}: Regenerate Albino Troll.")
        .Power(3)
        .Toughness(3)
        .Echo("{1}{G}")
        .Regenerate(cost: "{1}{G}".Parse(), text: "{1}{G}: Regenerate Albino Troll.");
    }
  }
}