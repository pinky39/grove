namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Hush : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new FirstMain());
          });
    }
  }
}