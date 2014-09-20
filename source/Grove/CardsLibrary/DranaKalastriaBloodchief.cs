namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.CostRules;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;

  public class DranaKalastriaBloodchief : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Drana, Kalastria Bloodchief")
        .ManaCost("{3}{B}{B}")
        .Type("Legendary Creature - Vampire Shaman")
        .Text(
          "{Flying}{EOL}{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.")
        .Power(4)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.";
            p.Cost = new PayMana("{B}{B}".Parse(), ManaUsage.Abilities, hasX: true);

            p.Effect = () => new ApplyModifiersToSelfAndToTargets(
              self: () => new AddPowerAndToughness(Value.PlusX, 0) {UntilEot = true},
              target: () => new AddPowerAndToughness(0, Value.MinusX) {UntilEot = true},
              toughnessReductionTargets: Value.PlusX);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness());
            p.CostRule(new XIsTargetsLifepointsLeft());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
          });
    }
  }
}