namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI;
  using Gameplay.AI.TargetingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class IronWill : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Iron Will")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Target creature gets +0/+4 until end of turn.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(0, 4) {UntilEot = true})
              .SetTags(EffectTag.IncreaseToughness);

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectPumpInstant(0, 4));
          });
    }
  }
}