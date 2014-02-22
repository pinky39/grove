namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class BurstOfEnergy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Burst of Energy")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Untap target permanent")
        .FlavorText("I stand ready to die for our world. Who will stand with me?")
        .Cast(p =>
          {
            p.Effect = () => new UntapTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card().On.Battlefield());

            p.TimingRule(new Any(
              new OnFirstMain(),
              new AfterOpponentDeclaresAttackers()));

            p.TargetingRule(new EffectUntapPermanent());
          });
    }
  }
}