namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;
  using Grove.Modifiers;
  using Grove.Triggers;

  public class OpalChampion : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Champion")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "When an opponent casts a creature spell, if Opal Champion is an enchantment, Opal Champion becomes a 3/3 Knight creature with first strike.")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent casts a creature spell, if Opal Champion is an enchantment, Opal Champion becomes a 3/3 Knight creature with first strike.";
            p.Trigger(new OnCastedSpell(
              filter: (ability, card) =>
                ability.OwningCard.Controller != card.Controller && ability.OwningCard.Is().Enchantment &&
                  card.Is().Creature));
         
            p.Effect = () => new ApplyModifiersToSelf
              (() => new ChangeToCreature(
                power: 3,
                toughness: 3,
                type: "Creature Knight",
                colors: L(CardColor.White)),
                () => new AddStaticAbility(Static.FirstStrike));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}