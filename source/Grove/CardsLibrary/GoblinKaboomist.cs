namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Triggers;

  public class GoblinKaboomist : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Goblin Kaboomist")
          .ManaCost("{1}{R}")
          .Type("Creature — Goblin Warrior")
          .Text("At the beginning of your upkeep, put a colorless artifact token named Land Mine onto the battlefield with \"{R}, Sacrifice this artifact: This artifact deals 2 damage to target attacking creature without flying.\" Then flip a coin. If you lose the flip, Goblin Kaboomist deals 2 damage to itself.")
          .Power(1)
          .Toughness(2)
          .TriggeredAbility(p =>
          {
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new CompoundEffect(
              new CreateTokens(
                count: 1,
                token: Card
                  .Named("Land Mine")
                  .Type("Token Artifact")
                  .Text("{R}, Sacrifice this artifact: This artifact deals 2 damage to target attacking creature without flying.")
                  .Colors(CardColor.Colorless)
                  .ActivatedAbility(ap =>
                  {
                    ap.Text = "{R}, Sacrifice this artifact: This artifact deals 2 damage to target attacking creature without flying.";
                    ap.Cost = new AggregateCost(
                      new PayMana(Mana.Red),
                      new Sacrifice());
                    ap.Effect = () => new DealDamageToTargets(2);
                    ap.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.IsAttacker && !c.Has().Flying));
                    ap.TargetingRule(new EffectDealDamage(2));
                    ap.TimingRule(new AfterOpponentDeclaresAttackers());
                  })),
              new FlipCoinDealDamageToItself(2));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}
