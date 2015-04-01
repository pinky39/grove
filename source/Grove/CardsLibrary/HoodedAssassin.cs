namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class HoodedAssassin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hooded Assassin")
        .ManaCost("{2}{B}")
        .Type("Creature — Human Assassin")
        .Text("When Hooded Assassin enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Hooded Assassin.{EOL}• Destroy target creature that was dealt damage this turn.")
        .Power(1)
        .Toughness(2)
        .TriggeredAbility(p =>
        {
          p.Text = "When Hooded Assassin enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Hooded Assassin.{EOL}• Destroy target creature that was dealt damage this turn.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Opponent.Battlefield.Creatures.Any(c => ctx.Turn.Events.HasBeenDamaged(c)),
          });
          p.Effect = () => new ChooseOneEffectFromGiven(
            message: "Press 'Yes' to put a +1/+1 counter on Hooded Assassin. Press 'No' to destroy target creature that was dealt damage this turn.",
            ifTrueEffect: new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1)),
            ifFalseEffect: new DestroyTargetPermanents(), 
            chooseAi: (e, g) =>
            {
              // TODO: Add tweaks for choosing first effect
              return !e.Controller.Opponent.Battlefield.Creatures.Any(c => g.Turn.Events.HasBeenDamaged(c));
            });
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Game.Turn.Events.HasBeenDamaged(c)).On.Battlefield());
          p.TargetingRule(new EffectDestroy());
        })
        .TriggeredAbility(p =>
        {
          p.Text = "When Hooded Assassin enters the battlefield, choose one —{EOL}• Put a +1/+1 counter on Hooded Assassin.{EOL}• Destroy target creature that was dealt damage this turn.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield)
          {
            Condition = ctx => ctx.Opponent.Battlefield.Creatures.All(c => !ctx.Turn.Events.HasBeenDamaged(c)),
          });
          p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new PowerToughness(1, 1), count: 1));
        });
    }
  }
}
