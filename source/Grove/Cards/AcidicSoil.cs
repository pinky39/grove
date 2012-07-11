namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

  public class AcidicSoil : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Acidic Soil")
        .ManaCost("{3}{R}")
        .Type("Sorcery")
        .Text("Acidic Soil deals damage to each player equal to the number of lands he or she controls.")
        .FlavorText("Phyrexia had tried to take Urza's soul. He was relieved that Shiv tried to claim only his soles.")
        .Timing(Timings.FirstMain())
        .Effect<DealDamageToEach>((e, _) => e.AmountPlayer = (player) => player.Battlefield.Lands.Count());
    }
  }
}