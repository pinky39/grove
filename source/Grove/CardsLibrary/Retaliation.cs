namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

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
                var tp = new TriggeredAbility.Parameters
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