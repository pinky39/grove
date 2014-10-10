namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;
  using Modifiers;

  public class EphemeralShields : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ephemeral Shields")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Target creature gains indestructible until end of turn.{I}(Damage and effects that say \"destroy\" don't destroy it.){/I}")
        .FlavorText("\"Even your shadow is too foul to tolerate.\"")
        .Convoke()
        .Cast(p =>
        {
          p.Text = "Target creature gains indestructible until end of turn.";

          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddStaticAbility(Static.Indestructible) { UntilEot = true }).SetTags(
              EffectTag.Indestructible);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectGiveIndestructible());
        });
    }
  }
}
