namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Triggers;

  public class HiddenAncients : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hidden Ancients")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.")
        .FlavorText("The only alert the invaders had was the rustling of leaves on a day without wind.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts an enchantment spell, if Hidden Ancients is an enchantment, Hidden Ancients becomes a 5/5 Treefolk creature.";

            p.Trigger(new OnCastedSpell(
              filter: (ability, card) => ability.OwningCard.Controller != card.Controller &&
                ability.OwningCard.Is().Enchantment && card.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(() => new Core.Modifiers.ChangeToCreature(
              power: 5,
              toughness: 5,
              type: "Creature Treefolk",
              colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}