namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class HiddenStag : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Stag")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.{EOL}Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.";

            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Land));

            p.Effect = () => new ApplyModifiersToSelf(() => new Core.Modifiers.ChangeToCreature(
              power: 3,
              toughness: 2,
              type: "Creature Elk Beast",
              colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller == card.Controller && ability.OwningCard.Is().Creature && card.Is().Land));

            p.Effect = () => new RemoveModifier(typeof (Core.Modifiers.ChangeToCreature));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}