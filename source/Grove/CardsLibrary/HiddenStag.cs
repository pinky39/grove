namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class HiddenStag : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Hidden Stag")
        .ManaCost("{1}{G}")
        .Type("Enchantment")
        .Text(
          "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.{EOL}Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "Whenever an opponent plays a land, if Hidden Stag is an enchantment, Hidden Stag becomes a 3/2 Elk Beast creature.";

            p.Trigger(new OnLandPlayed(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Land));

            p.Effect = () => new ApplyModifiersToSelf(() => new ChangeToCreature(
              power: 3,
              toughness: 2,
              type: "Creature Elk Beast",
              colors: L(CardColor.Green)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .TriggeredAbility(p =>
          {
            p.Text = "Whenever you play a land, if Hidden Stag is a creature, Hidden Stag becomes an enchantment.";
            p.Trigger(new OnLandPlayed(
              filter: (ability, card) =>
                ability.OwningCard.Controller == card.Controller && ability.OwningCard.Is().Creature && card.Is().Land));

            p.Effect = () => new RemoveModifier(typeof (ChangeToCreature));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}