namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Triggers;
  using Core.Zones;

  public class GraveTitan : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
        .StaticAbilities(Static.Deathtouch)
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever Grave Titan enters the battlefield or attacks, put two 2/2 black Zombie creature tokens onto the battlefield.";
            p.Trigger(new OnZoneChanged(to: Zone.Battlefield));
            p.Trigger(new OnAttack());

            p.Effect = () => new CreateTokens(
              count: 2,
              tokens:
                Card
                  .Named("Zombie Token")
                  .FlavorText(
                    "'Your brain is rotting?!.'{EOL}'...enough.'{EOL}-Y.A, 'The seven zombies'")
                  .Power(2)
                  .Toughness(2)
                  .Type("Creature - Token - Zombie")
                  .Colors(ManaColors.Black));
          });
    }
  }
}