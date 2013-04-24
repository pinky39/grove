namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class HiddenHerd : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Herd")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent plays a nonbasic land, if Hidden Herd is an enchantment, Hidden Herd becomes a 3/3 Beast creature.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent plays a nonbasic land, if Hidden Herd is an enchantment, Hidden Herd becomes a 3/3 Beast creature.";

            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().NonBasicLand));

            p.Effect = () => new ApplyModifiersToSelf(() => new Gameplay.Modifiers.ChangeToCreature(
              power: 3,
              toughness: 3,
              type: "Creature Beast",
              colors: L(CardColor.Green)
              ));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}