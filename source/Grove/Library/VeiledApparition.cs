namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;
  using Grove.Gameplay.Triggers;

  public class VeiledApparition : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Veiled Apparition")
        .ManaCost("{1}{U}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a spell, if Veiled Apparition is an enchantment, Veiled Apparition becomes a 3/3 Illusion creature with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a spell, if Veiled Apparition is an enchantment, Veiled Apparition becomes a 3/3 Illusion creature with flying and 'At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.'";
            p.Trigger(new OnCastedSpell(
              filter:
                (ability, card) =>
                  ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 3,
                type: "Creature Illusion",
                colors: L(CardColor.Blue)),
              () => new AddStaticAbility(Static.Flying),
              () =>
                {
                  var tp = new TriggeredAbilityParameters();
                  tp.Text = "At the beginning of your upkeep, sacrifice Veiled Apparition unless you pay {1}{U}.";
                  tp.Trigger(new OnStepStart(Step.Upkeep));
                  tp.Effect =
                    () => new PayManaOrSacrifice("{1}{U}".Parse(), "Pay upkeep? (or sacrifice Veiled Apparition)");

                  return new AddTriggeredAbility(new TriggeredAbility(tp));
                });

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}