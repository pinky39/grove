namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class SacredArmory : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sacred Armory")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text("{2}: Target creature gets +1/+0 until end of turn.")
        .FlavorText("Arrive for worship. Leave for war.")
        .ActivatedAbility(p =>
          {
            p.Text = "{2}: Target creature gets +1/+0 until end of turn.";
            p.Cost = new PayMana(2.Colorless(), supportsRepetitions: true);
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 0) {UntilEot = true}).SetTags(EffectTag.IncreasePower);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectPumpInstant(1, 0));
            p.TimingRule(new Any(
              new BeforeYouDeclareAttackers(),
              new AfterOpponentDeclaresAttackers()));            
            p.RepetitionRule(new RepeatMaxTimes());
          });
    }
  }
}