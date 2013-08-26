namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class AngelicPage : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Angelic Page")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}{EOL}{T}: Target attacking or blocking creature gets +1/+1 until end of turn.")
        .FlavorText("If only every message were as perfect as its bearers.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Target attacking or blocking creature gets +1/+1 until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true}) {Category = EffectCategories.ToughnessIncrease};
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());
            p.TimingRule(new OnStep(Step.DeclareBlockers));
            p.TargetingRule(new EffectPumpAttackerOrBlocker(1, 1));
          }
        );
    }
  }
}