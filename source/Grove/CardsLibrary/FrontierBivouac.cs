namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class FrontierBivouac : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Frontier Bivouac")
        .Type("Land")
        .Text("Frontier Bivouac enters the battlefield tapped.{EOL}{T}: Add {G}, {U} or {R} to your mana pool.")
        .FlavorText("\"The most powerful dreams visit those who shelter in a dragon's skull.\"{EOL}—Chianul, Who Whispers Twice")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {G}, {U} or {R} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isRed: true, isGreen: true));
        });
    }
  }
}
