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

  public class BarrinMasterWizard : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Barrin, Master Wizard")
        .ManaCost("{1}{U}{U}")
        .Type("Legendary Creature Human Wizard")
        .Text("{2}, Sacrifice a permanent: Return target creature to its owner's hand.")
        .FlavorText(
          "'Knowledge is no more expensive than ignorance, and at least as satisfying.'{EOL}—Barrin, master wizard")
        .Power(1)
        .Toughness(1)        
        .Abilities(
          ActivatedAbility(
            "{2}, Sacrifice a permanent: Return target creature to its owner's hand.",
            Cost<PayMana, Sacrifice>(cost => cost.Amount = 2.Colorless()),
            Effect<Core.Cards.Effects.ReturnToHand>(),
            effectTarget: Target(
              Validators.Card(x => x.Is().Creature), 
              Zones.Battlefield(),
              text: "Select a creature to bounce."),
            costTarget: Target(
                Validators.Card(ControlledBy.SpellOwner),
                Zones.Battlefield(),
                text: "Select a permanent to sacrifice.",
                mustBeTargetable: false),
            targetingAi: TargetingAi.SacPermanentToBounce(),
            timing: Any(Timings.InstantRemovalTarget()))
        );
    }
  }
}