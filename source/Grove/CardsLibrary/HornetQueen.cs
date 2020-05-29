namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class HornetQueen : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hornet Queen")
        .ManaCost("{4}{G}{G}{G}")
        .Type("Creature — Insect")
        .Text(
          "{Flying}{EOL}{Deathtouch}{I}(Any amount of damage this deals to a creature is enough to destroy it.){/I}{EOL}When Hornet Queen enters the battlefield, put four 1/1 green Insect creature tokens with flying and deathtouch onto the battlefield.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Deathtouch, Static.Flying)
        .TriggeredAbility(p =>
          {
            p.Text =
              "When Hornet Queen enters the battlefield, put four 1/1 green Insect creature tokens with flying and deathtouch onto the battlefield.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));

            p.Effect = () => new CreateTokens(
              count: 4,
              token: Card
                .Named("Insect")
                .Power(1)
                .Toughness(1)
                .Type("Token Creature - Insect")
                .Text("{Flying}, {Deathtouch}")
                .Colors(CardColor.Green)
                .SimpleAbilities(Static.Flying, Static.Deathtouch));
          });
    }
  }
}