namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Messages;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class Somnophore : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Somnophore")
        .ManaCost("{2}{U}{U}")
        .Type("Creature - Illusion")
        .Text(
          "{Flying}{EOL}Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Somnophore deals damage to a player, tap target creature that player controls. That creature doesn't untap during its controller's untap step for as long as Somnophore remains on the battlefield.";
            p.Trigger(new OnDamageDealt(playerFilter: delegate { return true; }));

            p.Effect = () => new CompoundEffect(
              new TapTargets(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);
                  modifier.AddLifetime(new PermanentLeavesBattlefieldLifetime(l => l.Modifier.SourceCard));
                  return modifier;
                }));

            p.TargetSelector.AddEffect(trg =>
              {
                trg
                  .Is.Card(tp => tp.Target.Card().Is().Creature &&
                    tp.TriggerMessage<DamageHasBeenDealt>().Receiver == tp.Target.Controller())
                  .On.Battlefield();

                trg.Message = "Select a creature to tap.";
              });

            p.TargetingRule(new EffectGiveDoesNotUntap());
          }
        );
    }
  }
}