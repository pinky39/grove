namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI;
  using Gameplay.AI.CostRules;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

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