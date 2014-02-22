namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;

  public class SmolderingCrater : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Smoldering Crater")
        .Type("Land")
        .Text(
          "Smoldering Crater enters the battlefield tapped.{EOL}{T}: Add {R} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cast(p => p.Effect = () => new PutIntoPlay(tap: true))
        .Cycling("{2}")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Red);
          });
    }
  }
}