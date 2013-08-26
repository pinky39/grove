namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Purify : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Purify")
        .ManaCost("{3}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all artifacts and enchantments.")
        .FlavorText("Our Mother The sky was Her hair; the sun, Her face. She danced on the grass and in the hills.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllPermanents((e, c) => c.Is().Enchantment || c.Is().Artifact);
          });
    }
  }
}