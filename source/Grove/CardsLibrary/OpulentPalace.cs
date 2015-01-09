namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class OpulentPalace : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opulent Palace")
        .Type("Land")
        .Text("Opulent Palace enters the battlefield tapped.{EOL}{T}: Add {B}, {G} or {U} to your mana pool.")
        .FlavorText("The dense jungle surrenders to a lush and lavish expanse. At its center uncoil the spires of Qarsi Palace.")
        .Cast(p => p.Effect = () => new CastPermanent(tap: true))
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {B}, {G} or {U} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isBlack: true, isGreen: true));
        });
    }
  }
}
