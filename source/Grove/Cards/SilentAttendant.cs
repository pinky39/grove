namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

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
            C.Effect<GainLife>((e, _) => e.SetAmount(1)),
            timing: Timings.EndOfTurnOrBeforeDeath()
            )
        );
    }
  }
}