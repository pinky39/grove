namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Infrastructure;

  public class DrownedCatacomb : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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