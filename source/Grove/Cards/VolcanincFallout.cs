namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class VolcanincFallout : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Volcanic Fallout")
        .ManaCost("{1}{R}{R}")
        .Type("Instant")
        .Text(
          "Volcanic Fallout can't be countered.{EOL}Volcanic Fallout deals 2 damage to each creature and each player.")
        .FlavorText("'How can we outrun the sky?'{EOL}—Hadran, sunseeder of Naya")
        .Timing(Timings.MassRemovalInstant())        
        .Effect<DealDamageToEach>((e, _) =>
          {
            e.AmountPlayer = delegate { return 2; };
            e.AmountCreature = delegate { return 2; };          
            e.CanBeCountered = false;
        });
    }
  }
}