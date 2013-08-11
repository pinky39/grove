namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

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
              new FirstMain(),
              new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers)));

            p.TargetingRule(new UntapPermanents());
          });
    }
  }
}