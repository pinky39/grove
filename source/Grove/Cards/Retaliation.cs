namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class Retaliation : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Retaliation")
        .ManaCost("{2}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have 'Whenever this creature becomes blocked by a creature, this creature gets +1/+1 until end of turn.'")
        .FlavorText("A foul, metallic stench clogged Urza's senses. It was then he knew his brother was no more.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () =>
              {
                var tp = new TriggeredAbilityParameters
                  {
                    Text =
                      "Whenever this creature becomes blocked by a creature, this creature gets +1/+1 until end of turn.",
                    Effect = () => new ApplyModifiersToSelf(
                      () => new AddPowerAndToughness(1, 1) {UntilEot = true}),                    
                  };

                tp.Trigger(new OnBlock(becomesBlocked: true, triggerForEveryCreature: true));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              };

            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          });
    }
  }
}