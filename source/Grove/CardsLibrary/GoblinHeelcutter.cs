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
        .Dash("{2}{R}")
        .TriggeredAbility(p =>
        {
          p.Text = "Whenever Goblin Heelcutter attacks, target creature can't block this turn.";

          p.Trigger(new WhenThisAttacks());

          p.Effect = () => new ApplyModifiersToTargets(
              () => new AddSimpleAbility(Static.CannotBlock) { UntilEot = true });

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectTapCreature());
        });
    }
  }
}
