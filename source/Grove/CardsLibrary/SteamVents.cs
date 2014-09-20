namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.Triggers;

  public class SteamVents : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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