namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;
  using Core.Zones;

  public class WurmcoilEngine : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Wurmcoil Engine")
        .ManaCost("{6}")
        .Type("Artifact Creature - Wurm")
        .Text(
          "{Deathtouch}, {Lifelink}{EOL}When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.")
        .Power(6)
        .Toughness(6)
        .Abilities(
          Static.Deathtouch,
          Static.Lifelink,
          TriggeredAbility(
            "When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.",
            Trigger<OnZoneChanged>(t =>
              {
                t.From = Zone.Battlefield;
                t.To = Zone.Graveyard;
              }),
            Effect<CreateTokens>(e => e.Tokens(
              Card
                .Named("Wurm Token")
                .Text("{Deathtouch}")
                .FlavorText("When wurms aren't hungry{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(ManaColors.Colorless)
                .Abilities(Static.Deathtouch),
              Card
                .Named("Wurm Token")
                .Text("{Lifelink}")
                .FlavorText("When wurms aren't hungry{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(ManaColors.Colorless)
                .Abilities(Static.Lifelink)))));
    }
  }
}