namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Archivist : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Archivist")
        .ManaCost("{2}{U}{U}")
        .Type("Creature Human Wizard")
        .Text("{T}: Draw a card.")
        .FlavorText("Some do. Some teach. The rest look it up.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Draw a card.";
            p.Cost = new Tap();
            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Turn(active: true, passive: false));
            p.TimingRule(new StackIsEmpty());
            p.TimingRule(new FirstMain());
          });
    }
  }
}