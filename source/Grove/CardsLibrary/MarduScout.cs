namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MarduScout : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mardu Scout")
        .ManaCost("{R}{R}")
        .Type("Creature — Goblin Scout")
        .Text("Dash {1}{R}{I}(You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.){/I}")
        .FlavorText("The Mardu all enjoy war, but only the goblins make a game of it.")
        .Power(3)
        .Toughness(1)
        .Cast(p => { p.Effect = () => new CastPermanent(); })
        .Cast(p =>
        {
          p.Cost = new PayMana("{1}{R}".Parse());
          p.Text = "{{1}}{{R}}: Dash";
          p.Effect = () => new CompoundEffect(
            new CastPermanent(),
            new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Dash){UntilEot = true},
              () => new AddStaticAbility(Static.Haste){UntilEot = true}));
          p.TimingRule(new OnFirstMain());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.";
          p.Trigger(new OnStepStart(step: Step.EndOfTurn)
          {
            Condition = ctx => ctx.OwningCard.Has().Dash
          });

          p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
