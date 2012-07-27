namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Triggers;
  using Core.Details.Mana;
  using Core.Dsl;

  public class HiddenSpider : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Timing(Timings.SecondMain())
        .Abilities(
          C.TriggeredAbility(
            "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.",
            C.Trigger<SpellWasCast>((t, _) => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature &&
                  card.Has().Flying),
            C.Effect<ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              p.Builder.Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Tougness = 5;
                  m.Type = "Creature - Spider";
                  m.Colors = ManaColors.Green;
                }), 
              p.Builder.Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Reach))),               
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}