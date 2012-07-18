namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class SilentAttendant : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Silent Attendant")
        .Type("Creature Human Cleric")
        .ManaCost("{2}{W}")
        .Text("{T}: You gain 1 life.")
        .FlavorText(
          "'The answer to life should never be death; it should always be more life, wrapped tight around us like precious silks.'")
        .Power(0)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          C.ActivatedAbility(
            "{T}: You gain 1 life.",
            C.Cost<TapOwnerPayMana>((cost, _) => cost.TapOwner = true),
            C.Effect<GainLife>(e => e.Amount = 1),
            timing: Any(Timings.EndOfTurn(), Timings.BeforeDeath())
            )
        );
    }
  }
}