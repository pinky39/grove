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

  public class HiddenAncients : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Hidden Ancients")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.")
        .FlavorText("The only alert the invaders had was the rustling of leaves on a day without wind.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.",
            C.Trigger<SpellWasCast>((t, _) => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Enchantment),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 5;
                  m.Tougness = 5;
                  m.Type = "Creature - Treefolk";
                  m.Colors = ManaColors.Green;
                })
              ))
            , triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}