namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Attunement : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Attunement")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Return Attunement to its owner's hand: Draw three cards, then discard four cards.")
        .FlavorText("The solution can hide for only so long.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Return Attunement to its owner's hand: Draw three cards, then discard four cards.";
            p.Cost = new Gameplay.Card.Costs.ReturnToHand();
            p.Effect = () => new DrawCards(3, discardCount: 4);
            p.TimingRule(new Any(new EndOfTurn(), new OwningCardWillBeDestroyed()));
          });
    }
  }
}