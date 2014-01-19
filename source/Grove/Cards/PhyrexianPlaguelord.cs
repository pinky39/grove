namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class PhyrexianPlaguelord : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Phyrexian Plaguelord")
        .ManaCost("{3}{B}{B}")
        .Type("Creature Carrier")
        .Text(
          "{T}, Sacrifice Phyrexian Plaguelord: Target creature gets -4/-4 until end of turn.{EOL}Sacrifice a creature: Target creature gets -1/-1 until end of turn.")
        .FlavorText("The final stage of the illness: delirium, convulsions, and death.")
        .Power(4)
        .Toughness(4)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}, Sacrifice Phyrexian Plaguelord: Target creature gets -4/-4 until end of turn.";
            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-4, -4) {UntilEot = true}) {ToughnessReduction = 4};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectReduceToughness(4));
            p.TimingRule(new WhenOwningCardWillBeDestroyed(considerCombat: false));
          })
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice a creature: Target creature gets -1/-1 until end of turn.";
            p.Cost = new Sacrifice();

            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(-1, -1) {UntilEot = true}) {ToughnessReduction = 1};

            p.TargetSelector
              .AddCost(trg =>
                {
                  trg.Is.Creature(ControlledBy.SpellOwner).On.Battlefield();
                  trg.Message = "Select a creature to sacrifice.";
                })
              .AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new WhenStackIsNotEmpty() ));
            p.TargetingRule(new CostSacrificeToReduceToughness(1) {ConsiderTargetingSelf = false} );            
          });
    }
  }
}