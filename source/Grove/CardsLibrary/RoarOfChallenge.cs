namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class RoarOfChallenge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Roar of Challenge")
        .ManaCost("{2}{G}")
        .Type("Sorcery")
        .Text(
          "All creatures able to block target creature this turn do so.{EOL}{I}Ferocious{/I} — That creature gains indestructible until end of turn if you control a creature with power 4 or greater.")
        .Cast(p =>
        {
          p.Condition = (card, game) => card.Controller.Battlefield.Creatures.All(x => x.Power < 4);

          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddStaticAbility(Static.Lure) {UntilEot = true});

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectBigWithoutEvasions());
          p.TimingRule(new OnFirstMain());
        })
        .Cast(p =>
        {
          p.Condition = (card, game) => card.Controller.Battlefield.Creatures.Any(x => x.Power >= 4);

          p.Effect = () => new ApplyModifiersToTargets(
            () => new AddStaticAbility(Static.Lure) {UntilEot = true},
            () => new AddStaticAbility(Static.Indestructible) { UntilEot = true }).SetTags(EffectTag.Indestructible);

          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectBigWithoutEvasions());
          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
