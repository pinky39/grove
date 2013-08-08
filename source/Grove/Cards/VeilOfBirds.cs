namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;

  public class VeilOfBirds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veil of Birds")
        .ManaCost("{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veil of Birds is an enchantment, Veil of Birds becomes a 1/1 Bird creature with flying.")
        .FlavorText("When wind marries sky, even the bride's veil sings her praises.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veil of Birds is an enchantment, Veil of Birds becomes a 1/1 Bird creature with flying.";
            p.Trigger(new OnCastedSpell(
              filter:
                (ability, card) =>
                  ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new Gameplay.Modifiers.ChangeToCreature(
                power: 1,
                toughness: 1,
                type: "Creature Bird",
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.Flying));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}