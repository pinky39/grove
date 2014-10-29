namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class SandsteppeCitadel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sandsteppe Citadel")
        .Type("Land")
        .Text("Sandsteppe Citadel enters the battlefield tapped.{EOL}{T}: Add {W}, {B} or {G} to your mana pool.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {W}, {B} or {G} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack:true, isGreen: true, isWhite: true));
        });
    }
  }
}
