namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class DauntlessRiverMarshal : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dauntless River Marshal")
        .ManaCost("{1}{W}")
        .Type("Creature — Human Soldier")
        .Text("Dauntless River Marshal gets +1/+1 as long as you control an Island.{EOL}{3}{U}: Tap target creature.")
        .FlavorText("\"Thieves and squid squirm the same way when you capture them.\"")
        .Power(2)
        .Toughness(1)
        .StaticAbility(p =>
          {
            p.Modifier(() => new AddPowerAndToughness(1, 1));
            p.Condition = cond => cond.OwnerControlsPermanent(c => c.Is("island"));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "{3}{U}: Tap target creature.";

            p.Cost = new PayMana("{3}{U}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new TapTargets();
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new OnStep(Step.BeginningOfCombat));            
            p.TargetingRule(new EffectTapCreature());
          });
    }
  }
}