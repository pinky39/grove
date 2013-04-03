namespace Grove.Core.Effects
{
  using System;
  using System.Collections.Generic;
  using Counters;
  using Decisions.Results;
  using Modifiers;

  public class PayLifeAddCounters : CustomizableEffect
  {
    public override ChosenOptions ChooseOptions()
    {
      const int minLife = 8;
      const int maxPay = 5;

      var couldPay = (Controller.Life - minLife);

      if (couldPay < 0)
        couldPay = 0;

      var wouldPay = Math.Min(couldPay, maxPay);
      var option = (int) EffectChoiceOption.Zero + wouldPay;

      return new ChosenOptions((EffectChoiceOption) option);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var lifeToPay = (int) results.Options[0] - (int) EffectChoiceOption.Zero;

      Controller.Life -= lifeToPay;

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };
      
      var addCounters = new AddCounters(() => new ChargeCounter(), lifeToPay)
        .Initialize(p, Game);      
      
      Source.OwningCard.AddModifier(addCounters);
    }

    public override string GetText()
    {
      return "Pay #0 life.";
    }

    public override IEnumerable<EffectChoice> GetChoices()
    {
      var options = new List<EffectChoiceOption>();

      for (var i = (int) EffectChoiceOption.Zero; i <= (int) EffectChoiceOption.Ten; i++)
      {
        options.Add((EffectChoiceOption) i);
      }

      yield return new EffectChoice(
        options.ToArray()
        );
    }
  }
}