namespace Grove.Cards
{
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
        .Cast(p => p.Timing = All(Timings.FirstMain(), Timings.HasCardsInGraveyard(card => card.Is().Enchantment)))
        .Abilities(
          TriggeredAbility(
            "When Monk Idealist enters the battlefield, return target enchantment card from your graveyard to your hand.",
            Trigger<OnZoneChanged>(t => t.To = Zone.Battlefield),
            Effect<PutToHand>(),
            Target(
              Validators.Card(card => card.Is().Enchantment),
              Zones.YourGraveyard(),
              mustBeTargetable: false,
              text: "Select an enchantment in your graveyard."),
            TargetSelectorAi.OrderByScore(Controller.SpellOwner)
            )
        );
    }
  }
}