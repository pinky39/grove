namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class BroodKeeper : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Brood Keeper")
        .ManaCost("{3}{R}")
        .Type("Creature — Human Shaman")
        .Text(
          "Whenever an Aura becomes attached to Brood Keeper, put a 2/2 red Dragon creature token with flying onto the battlefield. It has \"{R}: This creature gets +1/+0 until end of turn.\"")
        .FlavorText("\"Come, little one. Unfurl your wings, fill your lungs, and release your first fiery breath.\"")
        .Power(2)
        .Toughness(3)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an Aura becomes attached to Brood Keeper, put a 2/2 red Dragon creature token with flying onto the battlefield. It has \"{R}: This creature gets +1/+0 until end of turn.\"";

            p.Trigger(new OnAttachmentAttached((c, ctx) => c.Is().Aura));

            p.Effect = () => new CreateTokens(
              count: 1,
              token: Card
                .Named("Dragon")
                .Type("Token Creature - Dragon")
                .Text("{Flying}{EOL}{R}: This creature gets +1/+0 until end of turn.")
                .Power(2)
                .Toughness(2)
                .Colors(CardColor.Red)
                .SimpleAbilities(Static.Flying)
                .Pump(
                  cost: Mana.Red,
                  text: "{R}: This creature gets +1/+0 until end of turn.",
                  powerIncrease: 1,
                  toughnessIncrease: 0));
          });
    }
  }
}