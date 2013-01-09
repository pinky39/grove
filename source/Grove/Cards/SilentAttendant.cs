namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class SilentAttendant : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Silent Attendant")
        .Type("Creature Human Cleric")
        .ManaCost("{2}{W}")
        .Text("{T}: You gain 1 life.")
        .FlavorText(
          "'The answer to life should never be death; it should always be more life, wrapped tight around us like precious silks.'")
        .Power(0)
        .Toughness(2)        
        .Abilities(
          ActivatedAbility(
            "{T}: You gain 1 life.",
            Cost<Tap>(),
            Effect<ControllerGainsLife>(e => e.Amount = 1),
            timing: Any(Timings.EndOfTurn(), Timings.BeforeDeath())
            )
        );
    }
  }
}