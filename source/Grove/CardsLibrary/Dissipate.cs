namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Dissipate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dissipate")
        .ManaCost("{1}{U}{U}")
        .Type("Instant")
        .Text("Counter target spell. If that spell is countered this way, exile it instead of putting it into its owner's graveyard.")
        .FlavorText("\"This abomination never belonged in our world. I'm merely setting it free.\"{EOL}—Dierk, geistmage")
        .Cast(p =>
        {
          p.Text = "Counter target spell. If that spell is countered this way, exile it instead of putting it into its owner's graveyard.";

          p.Effect = () => new CounterTargetSpell(ep => ep.ExileSpell = true);       

          p.TargetSelector.AddEffect(trg => trg.Is.CounterableSpell().On.Stack());

          p.TargetingRule(new EffectCounterspell());
          p.TimingRule(new WhenTopSpellIsCounterable());
        });
    }
  }
}
