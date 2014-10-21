namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;
  using Modifiers;
  using Triggers;

  public class LurkingJackals : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lurking Jackals")
        .ManaCost("{B}")
        .Type("Enchantment")
        .Text(
          "When an opponent has 10 or less life, if Lurking Jackals is an enchantment, it becomes a 3/2 Hound creature.")
        .FlavorText("Often it's not the hunter or the hunted but the opportunist who thrives in Rath.")
        .TriggeredAbility(p =>
          {
            p.Text =
              "When an opponent has 10 or less life, if Lurking Jackals is an enchantment, it becomes a 3/2 Hound creature.";
            p.Trigger(
              new OnLifepointsLeft(
                ability => ability.OwningCard.Is().Enchantment && ability.OwningCard.Controller.Opponent.Life <= 10));

            p.Effect = () => new ApplyModifiersToSelf(
              () => new ChangeToCreature(
                power: 3,
                toughness: 2,
                type: t => t.Change(baseTypes: "creature", subTypes: "hound"),
                colors: L(CardColor.Black)));

            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}