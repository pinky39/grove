namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;
  using Modifiers;
  using Triggers;

  public class HiddenPredators : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Predators")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent controls a creature with power 4 or greater, if Hidden Predators is an enchantment, Hidden Predators becomes a 4/4 Beast creature.";
            p.Trigger(new OnEffectResolved(
              filter: (ability, game) =>
                {
                  if (ability.OwningCard.Is().Enchantment == false)
                    return false;

                  return ability.OwningCard.Controller.Opponent
                    .Battlefield.Creatures.Any(x => x.Power >= 4);
                }));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToCreature(
              power: 4,
              toughness: 4,
              type: "Creature Beast",
              colors: L(CardColor.Green)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}