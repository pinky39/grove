namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

  public class RavenousBaloth : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Ravenous Baloth")
        .ManaCost("{2}{G}{G}")
        .Type("Creature - Beast")
        .Text("Sacrifice a Beast: You gain 4 life.")
        .FlavorText(
          "All we know about the Krosan Forest we have learned from those few who made it out alive.{EOL}—Elvish refugee")
        .Power(4)
        .Toughness(4)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "Sacrifice a Beast: You gain 4 life.",
            C.Cost<SacrificePermanent>(),
            C.Effect<GainLife>((e, _) => e.SetAmount(4)),
            costSelector: C.Selector(Selectors.Creature((c)=> c.Is("beast"), Controller.SpellOwner)),
            targetFilter: TargetFilters.CostSacrificeGainLife(),
            timing: Timings.NoRestrictions()));
    }
  }
}