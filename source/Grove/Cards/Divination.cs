namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Divination : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Divination")
        .ManaCost("{2}{U}")
        .Type("Sorcery")
        .Text("Draw two cards.")
        .FlavorText(
          "Even the House of Galan, who takes the most scholarly approach to the mystic traditions, has resorted to exploring more primitive methods in Avacyn's absence.")
        .Cast(p =>
          {
            p.Effect = () => new DrawCards(2);
            p.TimingRule(new FirstMain());
          });
    }
  }
}