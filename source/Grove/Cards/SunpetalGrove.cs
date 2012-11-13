namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
  using Infrastructure;

  public class SunpetalGrove : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Sunpetal Grove")
        .Type("Land")
        .Text(
          "Sunpetal Grove enters the battlefield tapped unless you control a Forest or a Plains.{EOL}{T}: Add {G} or {W} to your mana pool.")
        .Abilities(
          ManaAbility(
            new ManaUnit(ManaColors.White | ManaColors.Green),
            "{T}: Add {G} or {W} to your mana pool."
            ))
        .Effect<PutIntoPlay>(
          e => e.PutIntoPlayTapped = e.Controller.Battlefield.None(card => card.Is("forest") || card.Is("plains")))
        .Timing(Timings.Lands());
    }
  }
}