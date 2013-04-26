namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class ArgothianEnchantress : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Argothian Enchantress")
        .ManaCost("{1}{G}")
        .Type("Creature Human Druid")
        .Text(
          "{Shroud}(This permanent can't be the target of spells or abilities.){EOL}Whenever you cast an enchantment spell, draw a card.")
        .Power(0)
        .Toughness(1)
        .StaticAbilities(Static.Shroud)
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever you cast an enchantment spell, draw a card.";
            p.Trigger(new OnCastedSpell((ability, card) =>
              card.Controller == ability.OwningCard.Controller && card.Is().Enchantment));
            p.Effect = () => new DrawCards(1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}