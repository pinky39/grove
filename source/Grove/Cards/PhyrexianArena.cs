namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Card.Triggers;
  using Gameplay.Effects;
  using Gameplay.States;

  public class PhyrexianArena : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Phyrexian Arena")
        .ManaCost("{1}{B}{B}")
        .Type("Enchantment")
        .Text("At the beginning of your upkeep, you draw a card and you lose 1 life.")
        .FlavorText("An audience of one with the malice of thousands.")
        .Cast(p => p.TimingRule(new SecondMain()))
        .TriggeredAbility(p =>
          {
            p.Text = "At the beginning of your upkeep, you draw a card and you lose 1 life.";
            p.Trigger(new OnStepStart(step: Step.Upkeep));
            p.Effect = () => new DrawCards(count: 1, lifeloss: 1);
            p.TriggerOnlyIfOwningCardIsInPlay = true;
          });
    }
  }
}