namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class RavenousBaloth : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Ravenous Baloth")
        .ManaCost("{2}{G}{G}")
        .Type("Creature - Beast")
        .Text("Sacrifice a Beast: You gain 4 life.")
        .FlavorText(
          "All we know about the Krosan Forest we have learned from those few who made it out alive.{EOL}—Elvish refugee")
        .Power(4)
        .Toughness(4)        
        .Abilities(
          ActivatedAbility(
            "Sacrifice a Beast: You gain 4 life.",
            Cost<Sacrifice>(),
            Effect<GainLife>(e => e.Amount = 4),
            costValidator: Target(
              Validators.Card(card => card.Is("beast"), Controller.SpellOwner),
              Zones.Battlefield(),
              mustBeTargetable: false),
            targetSelectorAi: TargetSelectorAi.CostSacrificeGainLife(),
            timing: Timings.NoRestrictions()));
    }
  }
}