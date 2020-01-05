namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class ArmamentCorps : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Armament Corps")
        .ManaCost("{2}{W}{B}{G}")
        .Type("Creature — Human Soldier")
        .Text("When Armament Corps enters the battlefield, distribute two +1/+1 counters among one or two target creatures you control.")
        .FlavorText("The Abzan avoid extended supply lines by incorporating weapons stores into their battle formations.")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
          
          p.DistributeAmount = 2;
          p.Effect = () => new DistributeCountersAmongTargets(() => new PowerToughness(1, 1));

          p.TargetSelector.AddEffect(
            trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield(),
            trg => {
              trg.MinCount = 1;
              trg.MaxCount = 2;            
          });
          
          p.TargetingRule(new EffectOrCostRankBy(c => 
            -c.Score, 
            ControlledBy.SpellOwner));
        });
    }
  }
}
