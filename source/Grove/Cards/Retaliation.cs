namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;
  using Core.Triggers;

  public class Retaliation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Retaliation")
        .ManaCost("{2}{G}")
        .Type("Enchantment")
        .Text(
          "Creatures you control have 'Whenever this creature becomes blocked by a creature, this creature gets +1/+1 until end of turn.'")
        .FlavorText("A foul, metallic stench clogged Urza's senses. It was then he knew his brother was no more.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () =>
              {
                var tp = new TriggeredAbilityParameters
                  {
                    Text = "Whenever this creature becomes blocked by a creature, this creature gets +1/+1 until end of turn.",                     
                    Effect = () => new ApplyModifiersToSelf(
                      () => new AddPowerAndToughness(1, 1) {UntilEot = true}),
                    TriggerOnlyIfOwningCardIsInPlay = true
                  };


                tp.Trigger(new OnBlock(becomesBlocked: true));

                return new AddTriggeredAbility(new TriggeredAbility(tp));
              };

            p.CardFilter = (card, effect) => card.Controller == effect.Source.Controller && card.Is().Creature;
          });
    }
  }
}