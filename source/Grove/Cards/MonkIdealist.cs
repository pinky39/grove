namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;
  using Core.Targeting;
  using Core.Zones;

  public class MonkIdealist : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Monk Idealist")
        .ManaCost("{2}{W}")
        .Type("Creature - Human Monk Cleric")
        .Text(
          "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.")
        .FlavorText("Belief is the strongest mortar.")
        .Power(2)
        .Toughness(2)
        .Timing(All(Timings.FirstMain(), Timings.ControllerHasAtLeastOneCardInGraveyard(card => card.Is().Enchantment)))
        .Abilities(
          C.TriggeredAbility(
            "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.",
            C.Trigger<OnZoneChange>((t, _) => t.To = Zone.Battlefield),
            C.Effect<ReturnToHand>(e => e.ReturnTarget = true),
            effectValidator: C.Validator(
              Validators.CardInGraveyard(card => card.Is().Enchantment), mustBeTargetable: false, text: "Select an enchantment in your graveyard."),
            selectorAi: TargetSelectorAi.OrderByDescendingScore()
            )
        );
    }
  }
}