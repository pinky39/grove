namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Landslide : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Landslide")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text("Sacrifice any number of Mountains. Landslide deals that much damage to target player.")
        .FlavorText("Sometimes the mountain takes.")
        .Cast(p =>
          {
            p.Effect = () => new SacrificeToDealDamageToTarget(c => c.Is("mountain"));
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYouHavePermanents(c => c.Is("mountain")));            
            p.TargetingRule(new EffectOpponent());            
          });
    }
  }
}