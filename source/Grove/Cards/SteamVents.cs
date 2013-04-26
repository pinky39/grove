namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Zones;

  public class SteamVents : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Steam Vents")
        .Type("Land - Island Mountain")
        .Text(
          "{T}: Add {U} or {R} to your mana pool.{EOL}As Steam Vents enters the battlefield, you may pay 2 life. If you don't, Steam Vents enters the battlefield tapped.")
        .TriggeredAbility(p =>
          {
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Effect = () => new PayLifeOrTapLand(2);
            p.UsesStack = false;
          })
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} or {R} to your mana pool.";
            p.ManaAmount(Mana.Colored(isBlue: true, isRed: true));
          });
    }
  }
}