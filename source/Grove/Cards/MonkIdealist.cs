namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class MonkIdealist : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Monk Idealist")
        .ManaCost("{2}{W}")
        .Type("Creature - Human Monk Cleric")
        .Text(
          "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.")
        .FlavorText("Belief is the strongest mortar.")
        .Power(2)
        .Toughness(2)
        .Timing(All(Timings.FirstMain(), Timings.HasCardsInGraveyard(card => card.Is().Enchantment)))
        .Abilities(
          TriggeredAbility(
            "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.",
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<ReturnToHand>(e => e.ReturnTarget = true),
            effectValidator: TargetValidator(
              TargetIs.CardInGraveyard(card => card.Is().Enchantment), mustBeTargetable: false, text: "Select an enchantment in your graveyard."),
            selectorAi: TargetSelectorAi.OrderByDescendingScore(Controller.SpellOwner)
            )
        );
    }
  }
}