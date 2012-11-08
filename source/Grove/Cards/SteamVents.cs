namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Zones;

  public class SteamVents : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Steam Vents")
        .Type("Land - Island Mountain")
        .Text(
          "{T}: Add {U} or {R} to your mana pool.{EOL}As Steam Vents enters the battlefield, you may pay 2 life. If you don't, Steam Vents enters the battlefield tapped.")
        .Timing(Timings.Lands())
        .Abilities(
          StaticAbility(
            Trigger<OnZoneChange>(t => t.To = Zone.Battlefield),
            Effect<PayLifeOrTap>(e => e.Life = 2)),
          ManaAbility(new ManaUnit(ManaColors.Blue | ManaColors.Red), "{T}: Add {U} or {R} to your mana pool."));
    }
  }
}