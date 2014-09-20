namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Hush : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {      
      yield return Card
        .Named("Hush")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("Destroy all enchantments.{EOL}{Cycling} {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyAllPermanents((e, card) => card.Is().Enchantment);
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}