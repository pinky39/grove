namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Effects;

  public class SeismicStrike : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Seismic Strike")
        .ManaCost("{2}{R}")
        .Type("Instant")
        .Text("Seismic Strike deals damage to target creature equal to the number of Mountains you control. ")
        .FlavorText("\"Life up here is simple. Adapt to the ways of the mountains and they will reward you. Fight them and they will end you.\"{EOL}—Kezim, prodigal pyromancer")
        .Cast(cp =>
        {
          cp.Effect = () => new DealDamageToTargets(
            amount: P(e => e.Controller.Battlefield.Count(x => x.Is("Mountain"))));

          cp.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          cp.TargetingRule(new EffectDealDamage(p =>
            p.Controller.Battlefield.Count(x => x.Is("Mountain"))));
        });
    }
  }
}
