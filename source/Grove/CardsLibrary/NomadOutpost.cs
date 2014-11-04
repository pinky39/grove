namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class NomadOutpost : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nomad Outpost")
        .Type("Land")
        .Text("Nomad Outpost enters the battlefield tapped.{EOL}{T}: Add {R}, {W} or {B} to your mana pool.")
        .FlavorText("\"Only the weak imprison themselves behind walls. We live free under the wind, and our freedom makes us strong.\"{EOL}—Zurgo, khan of the Mardu")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {R}, {W} or {B} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlack: true, isRed: true, isWhite: true));
        });
    }
  }
}
