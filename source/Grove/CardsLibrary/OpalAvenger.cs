namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class OpalAvenger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Opal Avenger")
        .ManaCost("{2}{W}")
        .Type("Enchantment")
        .Text(
          "When you have 10 or less life, if Opal Avenger is an enchantment, Opal Avenger becomes a 3/5 Soldier creature.")
        .FlavorText("As the sun grew cold in the realm, the statue grew warm.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When you have 10 or less life, if Opal Avenger is an enchantment, Opal Avenger becomes a 3/5 Soldier creature.";
            p.Trigger(
              new OnLifepointsLeft(
                ability => ability.OwningCard.Is().Enchantment && ability.OwningCard.Controller.Life <= 10));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 5,
                type: t => t.Change(baseTypes: "creature", subTypes: "soldier"),
                colors: L(CardColor.White)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}