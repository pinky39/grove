namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class DrownedCatacomb : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
       .Named("Drowned Catacomb")
       .Type("Land")
       .Text(
         "Drowned Catacomb enters the battlefield tapped unless you control an Island or a Swamp.{EOL}{T}: Add {U} or {B} to your mana pool.")
       .Abilities(
         C.ManaAbility(
           new Mana(ManaColors.Blue | ManaColors.Black),
           "{T}: Add {U} or {B} to your mana pool."
           ))
       .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped =
         player => !player.Battlefield.Any(card => card.Is("island") || card.Is("swamp")))
       .Timing(Timings.Lands());
    }
  }
}