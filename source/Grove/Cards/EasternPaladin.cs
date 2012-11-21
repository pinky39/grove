namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Core.Targeting;

  public class EasternPaladin : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
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
          ActivatedAbility(
            "{B}{B},{T}: Destroy target green creature.",
            Cost<TapOwnerPayMana>(cost =>
              {
                cost.Amount = "{B}{B}".ParseManaAmount();
                cost.TapOwner = true;                                
              }),
            Effect<DestroyTargetPermanents>(),            
            TargetValidator(TargetIs.Creature(card => card.HasColors(ManaColors.Green))),
            targetSelectorAi: TargetSelectorAi.Destroy(),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}