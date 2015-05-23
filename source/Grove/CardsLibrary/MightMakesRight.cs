namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MightMakesRight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Might Makes Right")
        .ManaCost("{5}{R}")
        .Type("Enchantment")
        .Text("At the beginning of combat on your turn, if you control each creature on the battlefield with the greatest power, gain control of target creature an opponent controls until end of turn. Untap that creature. It gains haste until end of turn. {I}(It can attack and {T} this turn.){/I}")
        .FlavorText("An oath of fealty sworn with a handshake.")
        .TriggeredAbility(p =>
        {
          p.Trigger(new OnStepStart(Step.BeginningOfCombat)
          {
            Condition = ctx =>
              {
                var yours = ctx.You.Battlefield.Creatures.Max(x => x.Power);
                var opponents = ctx.Opponent.Battlefield.Creatures.Max(x => x.Power);

                return yours > opponents;
              }
          });

          p.Effect = () => new CompoundEffect(
            new ApplyModifiersToTargets(
              () => new ChangeController(m => m.SourceCard.Controller) {UntilEot = true},
              () => new AddStaticAbility(Static.Haste) {UntilEot = true}),
            new UntapTargetPermanents());

          p.TargetSelector.AddEffect(trg => trg.Is.Creature(ControlledBy.Opponent).On.Battlefield());

          p.TargetingRule(new EffectGainControl());
          p.TriggerOnlyIfOwningCardIsInPlay = true;
        });
    }
  }
}
