namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class EasternPaladin : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Eastern Paladin")
        .ManaCost("{2}{B}{B}")
        .Type("Creature Zombie Knight")
        .Text("{B}{B},{T} : Destroy target green creature.")
        .FlavorText(
          "'Their fragile world. Their futile lives. They obstruct the Grand Evolution. In Yawgmoth's name, we shall excise them.'{EOL}—Oath of the East")
        .Power(3)
        .Toughness(3)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{B}{B},{T}: Destroy target green creature.",
            C.Cost<TapOwnerPayMana>((cost, c) =>
              {
                cost.Amount = "{B}{B}".ParseManaAmount();
                cost.TapOwner = true;                                
              }),
            C.Effect<DestroyTargetPermanents>(),            
            C.Validator(Validators.Creature(card => card.HasColors(ManaColors.Green))),
            selectorAi: TargetSelectorAi.Destroy(),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}