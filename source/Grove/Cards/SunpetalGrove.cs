namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class SunpetalGrove : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Sunpetal Grove")
        .Type("Land")
        .Text(
          "Sunpetal Grove enters the battlefield tapped unless you control a Forest or a Plains.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .Abilities(
          C.ManaAbility(
            new ManaUnit(ManaColors.White | ManaColors.Green),
            "{T}: Add {G} or {W} to your mana pool."
            ))
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped =
          player => !player.Battlefield.Any(card => card.Is("forest") || card.Is("plains")))
        .Timing(Timings.Lands());
    }
  }
}