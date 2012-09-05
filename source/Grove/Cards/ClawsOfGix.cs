namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class ClawsOfGix : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Claws of Gix")
        .ManaCost("{0}")
        .Type("Artifact")
        .Text("{1}, Sacrifice a permanent: You gain 1 life.")
        .FlavorText(
          "When the Brotherhood of Gix dug out the cave of Koilos they found their master's severed hand. They enshrined it, hoping that one day it would point the way to Phyrexia.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.ActivatedAbility(
            "{1}, Sacrifice a permanent: You gain 1 life.",
            C.Cost<SacPermanentPayMana>(cost => cost.Amount = 1.AsColorlessMana()),
            C.Effect<GainLife>(e => e.Amount = 1),
            costValidator:
              C.Validator(Validators.Permanent(controller: Controller.SpellOwner),
                text: "Select a permanent to sacrifice."),
            selectorAi: TargetSelectorAi.CostSacrificeGainLife()
            )
        );
    }
  }
}