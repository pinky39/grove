namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Triggers;
  using Core.Zones;

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
        .Abilities(
          StaticAbility.Shroud,
          C.TriggeredAbility(
            "Whenever you cast an enchantment spell, draw a card.",
            C.Trigger<ChangeZone>((t, _) =>
              {
                t.From = Zone.Hand;
                t.To = Zone.Stack;
                t.Filter = (ability, card) => card.Controller == ability.Controller && card.Is().Enchantment;
              }),
            C.Effect<DrawCards>((e, _) => e.DrawCount = 1),
            triggerOnlyIfOwningCardIsInPlay: true
            )
        );
    }
  }
}