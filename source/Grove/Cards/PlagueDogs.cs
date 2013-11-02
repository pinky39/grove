namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.Triggers;
  using Gameplay.Zones;

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
              new PayMana(2.Colorless(), ManaUsage.Abilities),
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
            p.Trigger(new OnZoneChanged(from: Zone.Battlefield, to: Zone.Graveyard));
            p.TriggerOnlyIfOwningCardIsInPlay = true;

            p.Effect = () => new ApplyModifiersToPermanents(
              selector: (effect, card) => card.Is().Creature,
              modifiers: () => new AddPowerAndToughness(-1, -1) {UntilEot = true}) {ToughnessReduction = 1};
          });
    }
  }
}