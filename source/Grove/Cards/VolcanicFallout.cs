namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class VolcanicFallout : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Volcanic Fallout")
        .ManaCost("{1}{R}{R}")
        .Type("Instant")
        .Text(
          "Volcanic Fallout can't be countered.{EOL}Volcanic Fallout deals 2 damage to each creature and each player.")
        .FlavorText("'How can we outrun the sky?'{EOL}—Hadran, sunseeder of Naya")
        .Cast(p =>
          {
            p.Timing = Timings.MassRemovalInstantSpeed();
            p.Effect = Effect<Core.Effects.DealDamageToEach>(e =>
              {
                e.AmountPlayer = 2;
                e.AmountCreature = 2;
                e.CanBeCountered = false;
              });
          });
    }
  }
}