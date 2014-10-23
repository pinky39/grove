namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class TurnToFrog : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Turn to Frog")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text(
          "Until end of turn, target creature loses all abilities and becomes a blue Frog with base power and toughness 1/1.")
        .FlavorText("\"Ribbit.\"")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new ChangeToCreature(
                power: 1,
                toughness: 1,
                colors: L(CardColor.Blue),
                type: t => t.Change(subTypes: "frog")) {UntilEot = true},
              () => new DisableAbilities(activated: true, simple: true, triggered: true) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectDestroy());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.Humble, combatOnly: true));
          });
    }
  }
}