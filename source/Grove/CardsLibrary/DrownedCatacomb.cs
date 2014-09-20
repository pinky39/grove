namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Infrastructure;

  public class DrownedCatacomb : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Drowned Catacomb")
        .Type("Land")
        .Text(
          "Drowned Catacomb enters the battlefield tapped unless you control an Island or a Swamp.{EOL}{T}: Add {U} or {B} to your mana pool.")
        .Cast(
          p => p.Effect = () => new PutIntoPlay(
            tap: P(e => e.Controller.Battlefield.None(card => card.Is("island") || card.Is("swamp")))))
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} or {B} to your mana pool.";
            p.ManaAmount(Mana.Colored(isBlue: true, isBlack: true));
          });
    }
  }
}