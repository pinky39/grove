namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;

  public class GrimContest : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grim Contest")
        .ManaCost("{1}{B}{G}")
        .Type("Instant")
        .Text("Choose target creature you control and target creature an opponent controls. Each of those creatures deals damage equal to its toughness to the other.")
        .FlavorText("The invader hoped he could survive the beast's jaws and emerge through its rotting skin.")
        .Cast(p =>
        {
          p.Effect = () => new EachTargetDealsDamageEqualToItsToughnessToOther();

          p.TargetSelector.AddEffect(trg =>
          {
            trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
            trg.Message = "Select a target creature you control.";
          });

          p.TargetSelector.AddEffect(trg =>
          {
            trg.Is.Creature(ControlledBy.Opponent).On.Battlefield();
            trg.Message = "Select a target creature your oppenent controls.";
          });

          p.TargetingRule(new EffectFight());
        });
    }
  }
}
