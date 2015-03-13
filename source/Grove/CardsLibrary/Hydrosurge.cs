namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Hydrosurge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hydrosurge")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Target creature gets -5/-0 until end of turn.")
        .FlavorText("\"Thirsty?\"{EOL}—Drunvalus, hydromancer")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(() => new AddPowerAndToughness(-5, -0) {UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReducePower());
            p.TimingRule(new Any(new AfterOpponentDeclaresBlockers(), new AfterOpponentDeclaresAttackers()));
          });
    }
  }
}