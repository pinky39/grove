namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;

  public class BloodHost : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blood Host")
        .ManaCost("{3}{B}{B}")
        .Type("Creature - Vampire")
        .Text("{1}{B}, Sacrifice another creature: Put a +1/+1 counter on Blood Host and you gain 2 life.")
        .FlavorText("It would be ill-mannered to decline his invitation. It would be ill-advised to accept it.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{1}{B}, Sacrifice another creature: Put a +1/+1 counter on Blood Host and you gain 2 life.";

            p.Cost = new AggregateCost(
              new PayMana("{1}{B}".Parse(), ManaUsage.Abilities),
              new Sacrifice());

            p.Effect = () => new CompoundEffect(
              new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1))
                .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness),
              new ChangeLife(amount: 2, yours: true));            

            p.TargetSelector.AddCost(trg =>
              {
                trg
                  .Is.Creature(ControlledBy.SpellOwner, canTargetSelf: false)
                  .On.Battlefield();

                trg.Message = "Select a creature to sacrifice.";
              });            

            p.TimingRule(new PumpOwningCardTimingRule(1, 1));
            p.TargetingRule(new EffectOrCostRankBy(c => c.Score) {TargetLimit = 1});
          });
    }
  }
}