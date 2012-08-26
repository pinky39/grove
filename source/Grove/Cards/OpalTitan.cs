namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Messages;

  public class OpalTitan : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Opal Titan")
        .ManaCost("{2}{W}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "When an opponent casts a creature spell, if Opal Titan is an enchantment, Opal Titan becomes a 4/4 Giant creature with protection from each of that spell's colors.",
            C.Trigger<SpellWasCast>((t, _) => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 4;
                  m.Toughness = 4;
                  m.Type = "Creature - Giant";
                  m.Colors = ManaColors.White;
                }),
              p.Builder.Modifier<AddProtectionFromColors>(
                m => { m.Colors = p.Parameters.Trigger<PlayerHasCastASpell>().Card.Colors; })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}