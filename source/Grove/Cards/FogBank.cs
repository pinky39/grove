namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.DamageHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

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
        .SimpleAbilities(
          Static.Defender,
          Static.Flying)
        .StaticAbility(p => p.Modifier(() => new AddDamagePrevention(
          modifier => new PreventAllDamageToAndFromCreature(modifier.SourceCard))));
    }
  }
}