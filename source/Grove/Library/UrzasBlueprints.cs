namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class UrzasBlueprints : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Urza's Blueprints")
        .ManaCost("{6}")
        .Type("Artifact")
        .Text("{Echo} {6}{EOL}{T}: Draw a card.")
        .FlavorText("From concept to paper to reality.")
        .Echo("{6}")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Draw a card.";
            p.Cost = new Tap();
            p.Effect = () => new DrawCards(1);

            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenStackIsEmpty());            
          });
    }
  }
}