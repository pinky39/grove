namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class DromokaDunecaster : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dromoka Dunecaster")
        .ManaCost("{W}")
        .Type("Creature — Human Wizard")
        .Text("{1}{W}, {T}: Tap target creature without flying.")
        .FlavorText("\"The dragonlords rule the tempests of the skies. Here in the wastes, the storms are mine to command.\"")
        .Power(0)
        .Toughness(2)
        .ActivatedAbility(p =>
        {
          p.Text = "{1}{W},{T}: Tap target creature without flying.";
          p.Cost = new AggregateCost(
            new PayMana("{1}{W}".Parse()),
            new Tap());
          
          p.Effect = () => new TapTargets();

          p.TargetSelector.AddEffect(trg => trg
            .Is.Card(c => c.Is().Creature && !c.Has().Flying)
            .On.Battlefield());
       
          p.TimingRule(new OnStep(Step.BeginningOfCombat));
          p.TargetingRule(new EffectTapCreature());
        });
    }
  }
}
