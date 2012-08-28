namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class CrystalChimes : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Crystal Chimes")
        .ManaCost("{3}")
        .Type("Artifact")
        .Timing(Timings.FirstMain())
        .Text("{3},{T}, Sacrifice Crystal Chimes: Return all enchantment cards from your graveyard to your hand.")
        .FlavorText("As Serra was to learn, the peace and sanctity of her realm were as fragile as glass.")
        .Abilities(
          C.ActivatedAbility(
            "{3},{T}, Sacrifice Crystal Chimes: Return all enchantment cards from your graveyard to your hand.",
            C.Cost<TapAndSacOwnerPayMana>(cost => cost.Amount = 3.AsColorlessMana()),
            C.Effect<ReturnAllCardsInGraveyardToHand>(e => e.Filter = card => card.Is().Enchantment),
            timing: Timings.HasCardsInGraveyard(card => card.Is().Enchantment, count: 2))
        );
    }
  }
}