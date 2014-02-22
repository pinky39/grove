namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

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
              () => new AddPowerAndToughness(1, 1) {UntilEot = true}).SetTags(EffectTag.IncreasePower,
                EffectTag.IncreaseToughness);
            p.TargetSelector.AddEffect(trg => trg.Is.AttackerOrBlocker().On.Battlefield());
            p.TimingRule(new OnStep(Step.DeclareBlockers));
            p.TargetingRule(new EffectPumpAttackerOrBlocker(1, 1));
          }
        );
    }
  }
}