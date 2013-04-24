﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.CostRules;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;

  public class DranaKalastriaBloodchief : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Drana, Kalastria Bloodchief")
        .ManaCost("{3}{B}{B}")
        .Type("Legendary Creature - Vampire Shaman")
        .Text(
          "{Flying}{EOL}{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.")
        .Power(4)
        .Toughness(4)
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{X}{B}{B}: Target creature gets -0/-X until end of turn and Drana, Kalastria Bloodchief gets +X/+0 until end of turn.";
            p.Cost = new PayMana("{B}{B}".Parse(), ManaUsage.Abilities, hasX: true);

            p.Effect = () => new ApplyModifiersToSelfAndToTargets(
              self: L(() => new AddPowerAndToughness(Value.PlusX, 0) {UntilEot = true}),
              target: L(() => new AddPowerAndToughness(0, Value.MinusX) {UntilEot = true}),
              toughnessReductionTargets: Value.PlusX);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new ReduceToughness());
            p.CostRule(new TargetsLifepoints());
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}