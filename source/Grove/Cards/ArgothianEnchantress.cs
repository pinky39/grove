namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Triggers;
  using Core.Dsl;

  public class ArgothianEnchantress : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Argothian Enchantress")
        .ManaCost("{1}{G}")
        .Type("Creature Human Druid")
        .Text(
          "{Shroud}(This permanent can't be the target of spells or abilities.){EOL}Whenever you cast an enchantment spell, draw a card.")
        .Power(0)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Abilities(
          Static.Shroud,
          C.TriggeredAbility(
            "Whenever you cast an enchantment spell, draw a card.",
            C.Trigger<SpellWasCast>(
              (t, _) => { t.Filter = (ability, card) => card.Controller == ability.Controller && card.Is().Enchantment; }),
            C.Effect<DrawCards>((e, _) => e.DrawCount = 1),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}