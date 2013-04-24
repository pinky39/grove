namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;

  public class HiddenSpider : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Spider")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.")
        .FlavorText("It wants only to dress you in silk.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell with flying, if Hidden Spider is an enchantment, Hidden Spider becomes a 3/5 Spider creature with reach.";

            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature &&
                    card.Has().Flying));

            p.Effect = () => new ApplyModifiersToSelf(() => new Gameplay.Modifiers.ChangeToCreature(
              power: 3,
              toughness: 5,
              type: "Creature Spider",
              colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}