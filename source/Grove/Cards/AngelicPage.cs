namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class AngelicPage : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Angelic Page")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +1/+1 until end of turn.")
        .FlavorText("If only every message were as perfect as its bearers.")
        .Power(1)
        .Toughness(1)
        .StaticAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true}) {Category = EffectCategories.ToughnessIncrease};
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());
            p.TimingRule(new DeclareBlockers());
            p.TargetingRule(new PumpAttackerOrBlocker(1, 1));
          }
        );
    }
  }
}