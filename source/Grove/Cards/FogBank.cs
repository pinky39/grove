namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Damage;

  public class FogBank : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Fog Bank")
        .ManaCost("{1}{U}")
        .Type("Creature Wall")
        .Text("{Defender}, {Flying}{EOL}Prevent all combat damage that would be dealt to and dealt by Fog Bank.")
        .Power(0)
        .Toughness(2)
        .Prevention(() => new PreventCombatDamage())
        .Prevention(() => new PreventReceived(combatOnly: true))
        .StaticAbilities(
          Static.Defender,
          Static.Flying
        );
    }
  }
}