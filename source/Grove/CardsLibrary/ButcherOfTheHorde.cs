namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
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
        .Text("{Flying}{EOL}Sacrifice another creature: Butcher of the Horde gains your choice of vigilance, lifelink, or haste until end of turn.")
        .Power(5)
        .Toughness(4)
        .SimpleAbilities(Static.Flying)
        .ActivatedAbility(p =>
        {
          p.Text = "Sacrifice another creature: Butcher of the Horde gains vigilance until end of turn.";

          p.Cost = new Sacrifice();

          p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Vigilance) { UntilEot = true });          

          p.TargetSelector.AddCost(trg =>
          {
            trg
              .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield();

            trg.Message = "Select a creature to sacrifice.";
          });

          p.TimingRule(new BeforeYouDeclareAttackers());
          p.TimingRule(new WhenCardHas(c => !c.Has().Vigilance && !c.IsTapped));

          p.TargetingRule(new EffectOrCostRankBy(c => c.Score) { TargetLimit = 1 });
        })
        .ActivatedAbility(p =>
        {
          p.Text = "Sacrifice another creature: Butcher of the Horde gains lifelink until end of turn.";

          p.Cost = new Sacrifice();

          p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Lifelink) { UntilEot = true });

          p.TargetSelector.AddCost(trg =>
          {
            trg
              .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield();

            trg.Message = "Select a creature to sacrifice.";
          });

          p.TimingRule(new Any(new BeforeYouDeclareAttackers(), new AfterOpponentDeclaresAttackers()));
          p.TimingRule(new WhenCardHas(c => !c.Has().Lifelink && !c.IsTapped));

          p.TargetingRule(new EffectOrCostRankBy(c => c.Score) { TargetLimit = 1 });
        })
        .ActivatedAbility(p =>
        {
          p.Text = "Sacrifice another creature: Butcher of the Horde gains haste until end of turn.";

          p.Cost = new Sacrifice();

          p.Effect = () => new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Haste) { UntilEot = true });

          p.TargetSelector.AddCost(trg =>
          {
            trg
              .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
              .On.Battlefield();

            trg.Message = "Select a creature to sacrifice.";
          });

          p.TimingRule(new WhenCardHas(c => c.HasSummoningSickness));

          p.TargetingRule(new EffectOrCostRankBy(c => c.Score) { TargetLimit = 1 });
        });
    }
  }
}
