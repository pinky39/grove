namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;
  using Targeting;

  public class DiscardAllCardsOfChosenColor : CustomizableEffect
  {
    private static readonly List<Rel> Map = new List<Rel>
      {
        new Rel {Color = CardColor.White, Choice = EffectOption.White},
        new Rel {Color = CardColor.Blue, Choice = EffectOption.Blue},
        new Rel {Color = CardColor.Black, Choice = EffectOption.Black},
        new Rel {Color = CardColor.Red, Choice = EffectOption.Red},
        new Rel {Color = CardColor.Green, Choice = EffectOption.Green},
      };

    public override ChosenOptions ChooseResult(List<object> operations)
    {
      var color = Target.Player().Battlefield.GetMostCommonColor();      
      return new ChosenOptions(Map.Single(x => x.Color == color).Choice);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var color = Map.Single(x => x.Choice.Equals(results.Options[0])).Color;

      var cardsToDiscard = Target.Player().Hand.Where(
        x => x.HasColor(color)).ToList();

      foreach (var card in cardsToDiscard)
      {
        Target.Player().DiscardCard(card);
      }
    }

    public override string GetText()
    {
      return "Target player discards all #0 cards.";
    }

    public override IEnumerable<object> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.White,
        EffectOption.Blue,
        EffectOption.Black,
        EffectOption.Red,
        EffectOption.Green);
    }

    private class Rel
    {
      public EffectOption Choice;
      public CardColor Color;
    }
  }
}