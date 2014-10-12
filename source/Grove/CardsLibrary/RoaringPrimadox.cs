namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class RoaringPrimadox : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Roaring Primadox")
        .ManaCost("{3}{G}")
        .Type("Creature - Beast")
        .Text("At the beginning of your upkeep, return a creature you control to its owner's hand.")
        .FlavorText("\"They're easy enough to find. Question is, are you sure you want to find one?\"{EOL}—Juruk, Kalonian tracker")
        .Power(4)
        .Toughness(4)
        .TriggeredAbility(p =>
        {
          p.Text = "At the beginning of your upkeep, return a creature you control to its owner's hand.";
          p.Trigger(new OnStepStart(Step.Upkeep));
          p.Effect = () => new ReturnToHand();
          p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield());

          p.TargetingRule(new EffectOrCostRankBy(x => x.Score));          

          p.TriggerOnlyIfOwningCardIsInPlay = true;          
        });
    }
  }
}
