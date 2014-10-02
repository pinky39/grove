namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Triggers;

  public class GraveTitan : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grave Titan")
        .ManaCost("{4}{B}{B}")
        .Type("Creature - Giant")
        .Text(
          "{Deathtouch}{EOL}Whenever Grave Titan enters the battlefield or attacks, put two 2/2 black Zombie creature tokens onto the battlefield.")
        .FlavorText("Death in form and function.")
        .Power(6)
        .Toughness(6)
        .SimpleAbilities(Static.Deathtouch)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Grave Titan enters the battlefield or attacks, put two 2/2 black Zombie creature tokens onto the battlefield.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Trigger(new OnAttack());

            p.Effect = () => new CreateTokens(
              count: 2,
              token:
                Card
                  .Named("Zombie")
                  .FlavorText(
                    "Your brain is rotting? ...enough?")
                  .Power(2)
                  .Toughness(2)
                  .Type("Token Creature - Zombie")
                  .Colors(CardColor.Black));
          });
    }
  }
}