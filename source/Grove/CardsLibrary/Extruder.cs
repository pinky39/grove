namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class Extruder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Extruder")
        .ManaCost("{4}")
        .Type("Artifact Creature")
        .Text("{Echo} {4}{EOL}Sacrifice an artifact: Put a +1/+1 counter on target creature.")
        .FlavorText("As the invasion drew closer, Urza's means began to resemble Phyrexia's end.")
        .Power(4)
        .Toughness(3)
        .Echo("{4}")
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice an artifact: Put a +1/+1 counter on target creature.";
            p.Cost = new Sacrifice();

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddCounters(() => new PowerToughness(1, 1), 1));

            p.TargetSelector
              .AddCost(trg =>
                {
                  trg.Is.Card(c => c.Is().Artifact, ControlledBy.SpellOwner).On.Battlefield();
                  trg.Message = "Select an artifact to sacrifice.";
                })
              .AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Any(new AfterOpponentDeclaresBlockers(), new AfterYouDeclareBlockers()));
            p.TargetingRule(new CostSacrificeEffectPump(1, 1));
          });
    }
  }
}