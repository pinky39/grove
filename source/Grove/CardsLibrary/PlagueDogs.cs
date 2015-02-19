namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;
  using Modifiers;
  using Triggers;

  public class PlagueDogs : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plague Dogs")
        .ManaCost("{4}{B}")
        .Type("Creature Zombie Hound")
        .Text(
          "When Plague Dogs dies, all creatures get -1/-1 until end of turn.{EOL}{2}, Sacrifice Plague Dogs: Draw a card.")
        .FlavorText("They're not just retrievers, they're carriers.")
        .Power(3)
        .Toughness(3)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}, Sacrifice Plague Dogs: Draw a card.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless()),
              new Sacrifice());

            p.Effect = () => new DrawCards(1);

            p.TimingRule(new Any(
              new WhenOwningCardWillBeDestroyed(),
              new OnEndOfOpponentsTurn()));

            p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
          })
        .TriggeredAbility(p =>
          {
            p.Text = "When Plague Dogs dies, all creatures get -1/-1 until end of turn.";
            p.Trigger(new OnZoneChanged(@from: Zone.Battlefield, to: Zone.Graveyard));
            p.TriggerOnlyIfOwningCardIsInPlay = true;

            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (effect, card) => card.Is().Creature,
              modifiers: () => new AddPowerAndToughness(-1, -1) {UntilEot = true}) {ToughnessReduction = 1};
          });
    }
  }
}