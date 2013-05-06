namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Infrastructure;

  public class SunpetalGrove : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sunpetal Grove")
        .Type("Land")
        .Text(
          "Sunpetal Grove enters the battlefield tapped unless you control a Forest or a Plains.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .Cast(p => p.Effect = () => new PutIntoPlay(
          tap: P(e => e.Controller.Battlefield.None(card => card.Is("forest") || card.Is("plains")))))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} or {W} to your mana pool.";
            p.ManaAmount(Mana.Colored(isWhite: true, isGreen: true));
          });
    }
  }
}