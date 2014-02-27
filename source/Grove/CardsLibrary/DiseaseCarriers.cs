namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TargetingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class DiseaseCarriers : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disease Carriers")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Rat")
        .Text("When Disease Carriers dies, target creature gets -2/-2 until end of turn.")
        .FlavorText("Rath is a disease all its inhabitants carry.")
        .Power(2)
        .Toughness(2)
        .TriggeredAbility(p =>
          {
            p.Text = "When Disease Carriers dies, target creature gets -2/-2 until end of turn.";
            
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));            

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-2, -2) {UntilEot = true}) {ToughnessReduction = 2};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());                                    
            p.TargetingRule(new EffectReduceToughness(2));
          });
    }
  }
}