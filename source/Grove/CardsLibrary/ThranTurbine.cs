namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
  using Triggers;

  public class ThranTurbine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Turbine")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text(
          "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.")
        .FlavorText("When Urza asked the viashino what it did, they answered: 'It hums.'")
        .Cast(p => p.TimingRule(new OnSecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text =
              "At the beginning of your upkeep, you may add {1} or {2} to your mana pool. You can't spend this mana to cast spells.";
            p.Trigger(new OnStepStart(step: Step.Upkeep));
            p.Effect = () => new AddManaToPool(2.Colorless(), ManaUsage.Abilities) {TriggerOrderRule = TriggerOrder.Low};
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          }
        );
    }
  }
}