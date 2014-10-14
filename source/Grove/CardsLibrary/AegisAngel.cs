namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class AegisAngel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Aegis Angel")
        .ManaCost("{4}{W}{W}")
        .Type("Creature - Angel")
        .Text("{Flying}{EOL}When Aegis Angel enters the battlefield, another target permanent gains indestructible for as long as you control Aegis Angel.{I}(Effects that say \"destroy\" don't destroy it. A creature with indestructible can't be destroyed by damage.){/I}")
        .Power(5)
        .Toughness(5)
        .SimpleAbilities(Static.Flying)
        .TriggeredAbility(p =>
        {
          p.Text = "When Aegis Angel enters the battlefield, another target permanent gains indestructible for as long as you control Aegis Angel.";
          p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

          p.Effect = () => new ApplyModifiersToTargets(() =>
            {
              var modifier = new AddStaticAbility(Static.Indestructible);
              modifier.AddLifetime(new PermanentLeavesBattlefieldLifetime(l => l.Modifier.SourceCard));
              return modifier;
            }).SetTags(EffectTag.Indestructible);

          p.TargetSelector.AddEffect(trg =>
            trg
              .Is.Creature(canTargetSelf: false)
              .On.Battlefield());

          p.TargetingRule(new EffectGiveIndestructible());
        });
    }
  }
}
