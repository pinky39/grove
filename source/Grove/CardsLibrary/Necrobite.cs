namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class Necrobite : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Necrobite")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text("Target creature gains deathtouch until end of turn. Regenerate it. {I}(The next time that creature would be destroyed this turn, it isn't. Instead tap it, remove all damage from it, and remove it from combat. Any amount of damage a creature with deathtouch deals to a creature is enough to destroy it.){/I}")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new ApplyModifiersToTargets(() => new AddStaticAbility(Static.Deathtouch){UntilEot = true}),
            new RegenerateTarget());

          p.TargetSelector.AddEffect(s => s.Is.Creature());

          p.TimingRule(new RegenerateTargetTimingRule());
          p.TargetingRule(new EffectGiveRegenerate());
        });
    }
  }
}
