namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;
  using Core.Messages;
  using Core.Modifiers;
  using Core.Triggers;

  public class OpalTitan : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Opal Titan")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            Effect<Core.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 4;
                  m.Toughness = 4;
                  m.Type = "Creature - Giant";
                  m.Colors = ManaColors.White;
                }),
              Modifier<AddProtectionFromColors>(
                m => { m.Colors = p.Parameters.Trigger<PlayerHasCastASpell>().Card.Colors; })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}