namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Attunement : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Attunement")
        .ManaCost("{2}{U}")
        .Type("Enchantment")
        .Text("Return Attunement to its owner's hand: Draw three cards, then discard four cards.")
        .FlavorText("The solution can hide for only so long.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "Return Attunement to its owner's hand: Draw three cards, then discard four cards.";
            p.Cost = new Gameplay.Costs.ReturnToHand();
            p.Effect = () => new DrawCards(3, discardCount: 4);
            p.TimingRule(new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed()));
          });
    }
  }
}