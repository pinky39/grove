namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class BraidwoodCup : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Braidwood Cup")
        .Type("Artifact")
        .ManaCost("{3}")
        .Text("{T}: You gain 1 life.")
        .FlavorText(
          "'I think it no accident that every civilized people has discovered the art of distillation.'")        
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: You gain 1 life.";
            p.Cost = new Tap();
            p.Effect = () => new ChangeLife(amount: 1, whos: P(e => e.Controller));
            p.TimingRule(new Any(new OnEndOfOpponentsTurn(), new WhenOwningCardWillBeDestroyed()));
          });
    }
  }
}