namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class ButcherOfTheHorde : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Butcher of the Horde")
        .ManaCost("{1}{R}{W}{B}")
        .Type("Creature — Demon")
        .Text(
          "{Flying}{EOL}Sacrifice another creature: Butcher of the Horde gains your choice of vigilance, lifelink, or haste until end of turn.")
        .Power(5)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
          {
            p.Text = "Sacrifice another creature: Butcher of the Horde gains vigilance until end of turn.";
            p.Cost = new Sacrifice();
            p.Effect = () => new OwnerGainsVigilanceLifelinkOrHaste();

            p.TargetSelector.AddCost(
              trg => trg.Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false).On.Battlefield(),
              trg => trg.Message = "Select a creature to sacrifice.");

            p.TimingRule(new OnFirstMain());
            p.TargetingRule(new EffectOrCostRankBy(c => c.Score) { TargetLimit = 1 });
          });
    }
  }
}