namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class KillShot : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kill Shot")
        .ManaCost("{2}{W}")
        .Type("Instant")
        .Text("Destroy target attacking creature.")
        .FlavorText(
          "Mardu archers are trained in Dakla, the way of the bow. They never miss their target, no matter how small, how fast, or how far away.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();

            p.TargetSelector.AddEffect(
              trg => trg.Is.Card(c => c.Is().Creature && c.IsAttacker).On.Battlefield(),
              trg => { trg.Message = "Select target attacking creature."; });

            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectDestroy());
          });
    }
  }
}