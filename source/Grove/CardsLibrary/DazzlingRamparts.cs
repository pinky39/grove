namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class DazzlingRamparts : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dazzling Ramparts")
        .ManaCost("{4}{W}")
        .Type("Creature — Wall")
        .Text("{Defender}{EOL}{1}{W},{T}: Tap target creature.")
        .FlavorText("When Anafenza holds court under the First Tree, the gates of Mer-Ek are sealed. No safer place exists in all of Tarkir.")
        .Power(0)
        .Toughness(7)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{W},{T}: Tap target creature.";
          p.Cost = new AggregateCost(
            new PayMana("{1}{W}".Parse()),
            new Tap());
          p.Effect = () => new TapTargets();
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TimingRule(new OnStep(Step.BeginningOfCombat));
          p.TargetingRule(new EffectTapCreature());
        });
    }
  }
}
