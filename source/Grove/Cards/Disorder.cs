namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai.TimingRules;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Disorder : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Disorder")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Disorder deals 2 damage to each white creature and each player who controls a white creature.")
        .FlavorText("Then, just when the other guys were winnin', the sky threw up.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToCreaturesAndPlayers(
              amountPlayer: 2,
              amountCreature: 2,
              filterPlayer: (e, player) =>
                player.Battlefield.Any(card => card.Is().Creature && card.HasColor(CardColor.White)),
              filterCreature: (e, c) => c.HasColor(CardColor.White));

            p.TimingRule(new OpponentHasPermanents(c => c.Is().Creature && c.HasColor(CardColor.White)));
          });
    }
  }
}