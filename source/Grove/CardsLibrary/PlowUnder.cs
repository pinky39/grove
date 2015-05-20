namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class PlowUnder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plow Under")
        .ManaCost("{3}{G}{G}")
        .Type("Sorcery")
        .Text("Put two target lands on top of their owners' libraries.")
        .FlavorText("To renew the land, plow the land. To destroy the land, do nothing.")
        .Cast(p =>
          {
            p.Effect = () => new PutTargetsOnTopOfLibrary();
            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Land).On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectPutOnTopOfLibrary());
          });
    }
  }
}