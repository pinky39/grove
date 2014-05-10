namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class WakeofDestruction : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wake of Destruction")
        .ManaCost("{3}{R}{R}{R}")
        .Type("Sorcery")
        .Text("Destroy target land and all other lands with the same name as that land.")
        .FlavorText(
          "Land charred black, rivers boiled, Crops and wells alike despoiled, Mountains leveled, forests felled— Footprints of the beasts of Keld.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new DestroyTargetPermanents(),
              new DestroyAllPermanents((e, c) => e.Target.Card().Name == c.Name));

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());
            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectDestroyMostCommon());
          });
    }
  }
}