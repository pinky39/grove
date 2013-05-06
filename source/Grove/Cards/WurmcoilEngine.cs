namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Triggers;
  using Gameplay.Zones;

  public class WurmcoilEngine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Wurmcoil Engine")
        .ManaCost("{6}")
        .Type("Artifact Creature - Wurm")
        .Text(
          "{Deathtouch}, {Lifelink}{EOL}When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.")
        .Power(6)
        .Toughness(6)
        .StaticAbilities(Static.Deathtouch, Static.Lifelink)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Wurmcoil Engine dies, put a 3/3 colorless Wurm artifact creature token with deathtouch and a 3/3 colorless Wurm artifact creature token with lifelink onto the battlefield.";

            p.Trigger(new OnZoneChanged(
              from: Zone.Battlefield,
              to: Zone.Graveyard));

            p.Effect = () => new CreateTokens(
              Card
                .Named("Wurm Token")
                .Text("{Deathtouch}")
                .FlavorText("When wurms aren't hungry.{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(CardColor.Colorless)
                .StaticAbilities(Static.Deathtouch),
              Card
                .Named("Wurm Token")
                .Text("{Lifelink}")
                .FlavorText("When wurms aren't hungry.{EOL}—Nantuko expression meaning 'never'")
                .Power(3)
                .Toughness(3)
                .Type("Artifact Creature - Wurm Token")
                .Colors(CardColor.Colorless)
                .StaticAbilities(Static.Lifelink));
          });
    }
  }
}