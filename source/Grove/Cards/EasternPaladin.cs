namespace Grove.Cards
{
  using System;
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
        .Abilities(
          ActivatedAbility(
            "{B}{B},{T}: Destroy target green creature.",
            Cost<PayMana, Tap>(cost => cost.Amount = "{B}{B}".ParseMana()),
            Effect<DestroyTargetPermanents>(),            
            Target(
              Validators.Card(card => card.Is().Creature && card.HasColors(ManaColors.Green)),
              Zones.Battlefield()),
            targetingAi: TargetingAi.Destroy(),
            timing: Timings.InstantRemovalTarget()));
    }
  }
}