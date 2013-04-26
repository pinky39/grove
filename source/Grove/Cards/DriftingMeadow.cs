namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Mana;

  public class DriftingMeadow : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Drifting Meadow")
        .Type("Land")
        .Text(
          "Drifting Meadow enters the battlefield tapped.{EOL}{T}: Add {W} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool.";
            p.ManaAmount(Mana.White);
          });
    }
  }
}