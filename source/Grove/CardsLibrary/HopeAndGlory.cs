namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class HopeAndGlory : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hope and Glory")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Untap two target creatures. Each of them gets +1/+1 until end of turn.")
        .FlavorText("Serra ruled by faith. I cannot afford that luxury.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new UntapTargetPermanents(),
              new ApplyModifiersToTargets(() => new AddPowerAndToughness(1, 1) {UntilEot = true}));

            p.TargetSelector.AddEffect(
              trg => trg.Is.Creature().On.Battlefield(),
              trg => {                
                trg.MinCount = 2;
                trg.MaxCount = 2;
              });

            p.TimingRule(new AfterOpponentDeclaresAttackers());
            p.TargetingRule(new EffectUntapPermanent());
          });
    }
  }
}