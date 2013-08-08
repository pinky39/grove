namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;

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
        .Cast(p => p.TimingRule(new FirstMain()))
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