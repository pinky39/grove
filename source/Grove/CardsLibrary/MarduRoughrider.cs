namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class MarduRoughrider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
          .Named("Mardu Roughrider")
          .ManaCost("{2}{R}{W}{B}")
          .Type("Creature — Orc Warrior")
          .Text("Whenever Mardu Roughrider attacks, target creature can't block this turn.")
          .FlavorText("The most ferocious saddlebrutes lead the assault, ramming through massed pikes and stout barricades as if they were paper and silk.")
          .Power(5)
          .Toughness(4)
          .TriggeredAbility(p =>
          {
            p.Text = "Whenever Mardu Roughrider attacks, target creature can't block this turn.";

            p.Trigger(new WhenThisAttacks());

            p.Effect = () => new ApplyModifiersToTargets(
                () => new AddStaticAbility(Static.CannotBlock) { UntilEot = true });

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectTapCreature());
          });
    }
  }
}
