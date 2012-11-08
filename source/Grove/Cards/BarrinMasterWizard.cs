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
        .Timing(Timings.FirstMain())
        .Abilities(
          ActivatedAbility(
            "{2}, Sacrifice a permanent: Return target creature to its owner's hand.",
            Cost<SacPermanentPayMana>(cost => cost.Amount = 2.AsColorlessMana()),
            Effect<ReturnToHand>(e => e.ReturnTarget = true),
            effectValidator: Validator(Validators.Creature(), text: "Select a creature to bounce."),
            costValidator:
              Validator(Validators.Permanent(controller: Controller.SpellOwner),
                text: "Select a permanent to sacrifice.",
                mustBeTargetable: false),
            selectorAi: TargetSelectorAi.SacPermanentToBounce(),
            timing: Any(Timings.InstantRemovalTarget()))
        );
    }
  }
}