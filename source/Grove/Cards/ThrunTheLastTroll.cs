namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

  public class ThrunTheLastTroll : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Thrun, the Last Troll")
        .ManaCost("{2}{G}{G}")
        .Type("Legendary Creature - Troll Shaman")
        .Text(
          "Thrun can't be countered.{EOL}Thrun can't be the target of spells or abilities your opponents control.{EOL}{1}{G}: Regenerate Thrun.")
        .FlavorText("His crime was silence, and now he suffers it eternally.")
        .Power(4)
        .Toughness(4)
        .Effect<PutIntoPlay>((e, _) => e.CanBeCountered = false)
        .Abilities(
          StaticAbility.Hexproof,
          C.ActivatedAbility(
            "{1}{G}: Regenerate Thrun.",
            C.Cost<TapOwnerPayMana>((c, _) => c.Amount = "{1}{G}".ParseManaAmount()),
            C.Effect<Regenerate>(),
            timing: Timings.RegenerateThis()));
    }
  }
}