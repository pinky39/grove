namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Humble : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Humble")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Target creature loses all abilities and becomes 0/1 until end of turn.")
        .FlavorText("'It is not your place to rule, Radiant. It may not even be mine.'{EOL}—Serra")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new DisableAbilities {UntilEot = true},
              () => new SetPowerAndToughness(0, 1) {UntilEot = true});

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new Destroy());
            p.TimingRule(new TargetRemoval(combatOnly: true));
          });
    }
  }
}