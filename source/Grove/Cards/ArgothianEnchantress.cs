namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Cards;
  using Core.Cards.Effects;
  using Core.Cards.Triggers;
  using Core.Dsl;

  public class ArgothianEnchantress : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Enchantress")
        .ManaCost("{1}{G}")
        .Type("Creature Human Druid")
        .Text(
          "{Shroud}(This permanent can't be the target of spells or abilities.){EOL}Whenever you cast an enchantment spell, draw a card.")
        .Power(0)
        .Toughness(1)
        .Abilities(
          Static.Shroud,
          TriggeredAbility(
            "Whenever you cast an enchantment spell, draw a card.",
            Trigger<SpellWasCast>(
              t => { t.Filter = (ability, card) => card.Controller == ability.Controller && card.Is().Enchantment; }),
            Effect<DrawCards>(p => p.DrawCount = 1),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}