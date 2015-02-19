namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class GoblinHeelcutter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Heelcutter")
        .ManaCost("{3}{R}")
        .Type("Creature — Goblin Berserker")
        .Text("Whenever Goblin Heelcutter attacks, target creature can't block this turn.{EOL}Dash {2}{R}{I}(You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.){/I}")
        .FlavorText("The Mardu all enjoy war, but only the goblins make a game of it.")
        .Power(3)
        .Toughness(2)
        .Cast(p => { p.Effect = () => new CastPermanent(); })
        .Cast(p =>
        {
          p.Cost = new PayMana("{2}{R}".Parse());
          p.Text = "{{2}}{{R}}: Dash";
          p.Effect = () => new CompoundEffect(
            new CastPermanent(),
            new ApplyModifiersToSelf(
              () => new AddStaticAbility(Static.Dash) { UntilEot = true },
              () => new AddStaticAbility(Static.Haste) { UntilEot = true }));
          p.TimingRule(new OnFirstMain());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "You may cast this spell for its dash cost. If you do, it gains haste, and it's returned from the battlefield to its owner's hand at the beginning of the next end step.";
          p.Trigger(new OnStepStart(step: Step.EndOfTurn)
          {
            Condition = (t, g) => t.OwningCard.Has().Dash
          });

          p.Effect = () => new Effects.ReturnToHand(returnOwningCard: true);
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        })
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Goblin Heelcutter attacks, target creature can't block this turn.";

          p.Trigger(new WhenThisAttacks());

          p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.CannotBlock) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectTapCreature());
        });
    }
  }
}
