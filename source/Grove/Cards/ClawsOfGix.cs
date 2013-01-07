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

  public class ClawsOfGix : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Claws of Gix")
        .ManaCost("{0}")
        .Type("Artifact")
        .Text("{1}, Sacrifice a permanent: You gain 1 life.")
        .FlavorText(
          "When the Brotherhood of Gix dug out the cave of Koilos they found their master's severed hand. They enshrined it, hoping that one day it would point the way to Phyrexia.")
        .Cast(p => p.Timing = All(Timings.FirstMain(), Timings.OnlyOneOfKind()))        
        .Abilities(
          ActivatedAbility(
            "{1}, Sacrifice a permanent: You gain 1 life.",
            Cost<PayMana, Sacrifice>(cost => cost.Amount = 1.Colorless()),
            Effect<GainLife>(e => e.Amount = 1),
            costValidator:
              Target(
                Validators.Card(controller: Controller.SpellOwner),
                Zones.Battlefield(),
                text: "Select a permanent to sacrifice.", mustBeTargetable: false),
            targetSelectorAi: TargetSelectorAi.CostSacrificeGainLife())
        );
    }
  }
}