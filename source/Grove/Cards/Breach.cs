namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class Breach : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Breach")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Target creature gets +2/+0 and gains fear until end of turn. (It can't be blocked except by artifact creatures and/or black creatures.)")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddStaticAbility(Static.Fear) {UntilEot = true},
              () => new AddPowerAndToughness(2, 0) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new Steps(Step.DeclareAttackers));
            p.TargetingRule(new GainEvasion());
          });
    }
  }
}