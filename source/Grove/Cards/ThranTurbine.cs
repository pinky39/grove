namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.Mana;
  using Gameplay.States;

  public class ThranTurbine : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Thran Turbine")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.")
        .FlavorText("When Urza asked the viashino what it did, they answered: 'It hums.'")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.";
            p.Trigger(new OnStepStart(step: Step.Upkeep, order: TriggerOrder.Last));
            p.Effect = () => new AddManaToPool(2.Colorless(), ManaUsage.Abilities);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}