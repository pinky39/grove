namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Infrastructure;

  public class DrownedCatacomb : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Drowned Catacomb")
        .Type("Land")
        .Text(
          "Drowned Catacomb enters the battlefield tapped unless you control an Island or a Swamp.{EOL}{T}: Add {U} or {B} to your mana pool.")
        .Cast(p => p.Effect = Effect<PutIntoPlay>(
          e => e.PutIntoPlayTapped = e.Controller.Battlefield.None(card => card.Is("island") || card.Is("swamp"))))
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Blue | ManaColors.Black),
            "{T}: Add {U} or {B} to your mana pool."
            ));
    }
  }
}