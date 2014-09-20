namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Triggers;

  public class UrzasIncubator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Urza's Incubator")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("As Urza's Incubator enters the battlefield, choose a creature type.{EOL}Creature spells of the chosen type cost {2} less to cast.")
        .FlavorText("Stop thinking like an artificer, Urza, and start thinking like a father.")
        .Cast(p => p.TimingRule(new OnFirstMain()))        
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new CreaturesOfChosenTypeCostLess(2.Colorless());
            p.UsesStack = false;            
          });
    }
  }
}