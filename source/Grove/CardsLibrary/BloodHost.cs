namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
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

                  p.TargetSelector.AddCost(trg =>
                  {
                      trg.Is.Creature(ControlledBy.SpellOwner).Is.Card(p1 => p1.OwningCard != p1.Target).On.Battlefield();
                      trg.Message = "Select a creature to sacrifice.";
                  });

                  p.TargetingRule(new EffectBounce());
                  p.TargetingRule(new EffectCombatEnchantment());                  

                  p.Effect = () => new CompoundEffect(
                      new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), 1)),
                      new YouGainLife(2)
                      );
              });
        }
    }
}
