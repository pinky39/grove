namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Costs;
  using Gameplay.Card.Counters;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class LotusBlossom : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Lotus Blossom")
        .ManaCost("{2}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.{EOL}{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you may put a petal counter on Lotus Blossom.";
            p.Trigger(new OnStepStart(Step.Upkeep));
            p.Effect = () => new ApplyModifiersToSelf(() => new AddCounters(() => new ChargeCounter(), 1));
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          })
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}, Sacrifice Lotus Blossom: Add X mana of any color to your mana pool, where X is the number of petal counters on Lotus Blossom.";

            p.Cost = new AggregateCost(
              new Tap(),
              new Sacrifice());

            p.Effect = () => new AddManaToPool(P(e => 
              Mana.Colored(ManaColor.Any, e.Source.OwningCard.Counters.GetValueOrDefault())));

            p.TimingRule(new ControllerNeedsAdditionalMana());              
          });
    }
  }
}