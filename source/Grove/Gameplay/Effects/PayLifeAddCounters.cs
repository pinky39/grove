namespace Grove.Gameplay.Effects
{
  using System;
  using System.Collections.Generic;
  using Counters;
  using Decisions.Results;
  using Modifiers;

  public class PayLifeAddCounters : CustomizableEffect
  {
    private readonly CounterType _counterType;

    private PayLifeAddCounters()
    {
      
    }

    public PayLifeAddCounters(CounterType counterType)
    {
      _counterType = counterType;
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      const int minLife = 8;
      const int maxPay = 5;

      var couldPay = (Controller.Life - minLife);

      if (couldPay < 0)
        couldPay = 0;

      var wouldPay = Math.Min(couldPay, maxPay);

      return new ChosenOptions(wouldPay);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var lifeToPay = (int) results.Options[0];

      Controller.Life -= lifeToPay;

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      var addCounters = new AddCounters(() => new SimpleCounter(_counterType), lifeToPay)
        .Initialize(p, Game);

      Source.OwningCard.AddModifier(addCounters);
    }

    public override string GetText()
    {
      return "Pay #0 life.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new RangeEffectChoice(0, 10);
    }
  }
}