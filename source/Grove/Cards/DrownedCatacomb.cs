namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;
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
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.Blue | ManaColors.Black),
            "{T}: Add {U} or {B} to your mana pool."
            ))
        .Effect<PutIntoPlay>(
          e => e.PutIntoPlayTapped = e.Controller.Battlefield.None(card => card.Is("island") || card.Is("swamp")))
        .Timing(Timings.Lands());
    }
  }
}