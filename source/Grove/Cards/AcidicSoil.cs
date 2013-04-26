namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class AcidicSoil : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Acidic Soil")
        .ManaCost("{3}{R}")
        .Type("Sorcery")
        .Text("Acidic Soil deals damage to each player equal to the number of lands he or she controls.")
        .FlavorText("Phyrexia had tried to take Urza's soul. He was relieved that Shiv tried to claim only his soles.")
        .Cast(p =>
          {
            p.TimingRule(new FirstMain());
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountPlayer: (e, player) => player.Battlefield.Lands.Count());
          });
    }
  }
}