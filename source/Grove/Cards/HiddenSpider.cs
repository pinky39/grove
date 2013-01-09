namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Modifiers;
  using Core.Cards.Triggers;
  using Core.Dsl;
  using Core.Mana;

  public class HiddenSpider : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Cast(p => p.Timing = Timings.SecondMain())
        .Abilities(
          TriggeredAbility(
            "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.",
            Trigger<OnCastedSpell>(t => t.Filter =
              (ability, card) =>
                ability.Controller != card.Controller && ability.OwningCard.Is().Enchantment && card.Is().Creature &&
                  card.Has().Flying),
            Effect<Core.Cards.Effects.ApplyModifiersToSelf>(p => p.Effect.Modifiers(
              Modifier<ChangeToCreature>(m =>
                {
                  m.Power = 3;
                  m.Toughness = 5;
                  m.Type = "Creature - Spider";
                  m.Colors = ManaColors.Green;
                }), 
              Modifier<AddStaticAbility>(m => m.StaticAbility = Static.Reach))),               
            triggerOnlyIfOwningCardIsInPlay: true)
        );
    }
  }
}