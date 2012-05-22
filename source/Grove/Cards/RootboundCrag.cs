namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Effects;

  public class RootboundCrag : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Rootbound Crag")
        .Type("Land")
        .Text(
          "Rootbound Crag enters the battlefield tapped unless you control a Mountain or a Forest.{EOL}{T}: Add {R} or {G} to your mana pool.")
        .Abilities(
          C.ManaAbility(
            new Mana(ManaColors.Red | ManaColors.Green),
            "{T}: Add {R} or {G} to your mana pool."
            ))
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped =
          player => !player.Battlefield.Any(card => card.Is("forest") || card.Is("mountain")))
        .Timing(Timings.Lands);
    }
  }
}