namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Effects;
  using Infrastructure;
  using Modifiers;
  using Triggers;

  public class AvenSurveyor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aven Surveyor")
        .ManaCost("{3}{U}{U}")
        .Type("Creature — Bird Scout")
        .Text("{Flying}{EOL}When Aven Surveyor enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Aven Surveyor.{EOL}• Return target creature to its owner's hand.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "When Aven Surveyor enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Aven Surveyor.{EOL}• Return target creature to its owner's hand.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Opponent.Battlefield.Creatures.Any(),
          });
          p.Effect = () => new ChooseOneEffectFromGiven(
            message: "Press 'Yes' to put a +1/+1 counter on Aven Surveyor. Press 'No' to return target creature to its owner's hand",
            ifTrueEffect: new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1)),
            ifFalseEffect: new ReturnToHand(), 
            chooseAi: (e, g) =>
            {
              // TODO: Add tweaks for choosing first effect
              return e.Controller.Opponent.Battlefield.Creatures.None();
            });
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectBounce());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Aven Surveyor enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Aven Surveyor.{EOL}• Return target creature to its owner's hand.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Opponent.Battlefield.Creatures.None(),
          });
          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1));
        });
    }
  }
}
