namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Soulmender : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Soulmender")
        .ManaCost("{W}")
        .Type("Creature - Human Cleric")
        .Text("{T}: You gain 1 life.")
        .FlavorText("\"Healing is more art than magic. Well, there is still quite a bit of magic.\"")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: You gain 1 life.";
            p.Cost = new Tap();
            p.Effect = () => new ChangeLife(amount: 1, yours: true);
            p.TimingRule(new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed()));
          });
    }
  }
}