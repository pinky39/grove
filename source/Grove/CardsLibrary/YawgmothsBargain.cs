namespace Grove.CardsLibrary
{
  using System;
  using System.Collections.Generic;
  using AI.RepetitionRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class YawgmothsBargain : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Yawgmoth's Bargain")
        .ManaCost("{4}{B}{B}")
        .Type("Enchantment")
        .Text("Skip your draw step.{EOL}Pay 1 life: Draw a card.")
        .FlavorText("He craves only one commodity.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .StaticAbility(p => p.Modifier(() => new SkipStep(Step.Draw)))
        .ActivatedAbility(p =>
          {
            p.Text = "Pay 1 life: Draw a card.";
            p.Cost = new PayLife(1, supportsRepetitions: true);
            p.Effect = () => new DrawCards(1);

            p.RepetitionRule(new RepeatMaxTimes(rp =>
              {                                                
                var controller = rp.Card.Controller;                                
                var count = controller.Hand.Count;                
                var fillCount = Math.Max(0, 7 - count);
                return Math.Min(controller.Life / 3, fillCount);
              }));

            p.TimingRule(new OnFirstMain());
          });
    }
  }
}