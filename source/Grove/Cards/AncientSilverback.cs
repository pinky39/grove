namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Misc;

  public class AncientSilverback : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ancient Silverback")
        .ManaCost("{4}{G}{G}")
        .Type("Creature Ape")
        .Text(
          "{G}: Regenerate Ancient Silverback")
        .FlavorText("The Phyrexian killing machines couldn't have known the seriousness of their mistake in wounding the ape—they'd never seen it angry.")
        .Power(6)
        .Toughness(5)
        .Regenerate(cost: "{G}".Parse(), text: "{G}: Regenerate Ancient Silverback");
    }
  }
}