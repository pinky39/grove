namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions.Results;
  using Mana;
  using Targeting;

  public class DiscardAllCardsOfChosenColor : CustomizableEffect
  {
    private static readonly List<Rel> Map = new List<Rel>
      {
        new Rel {Color = ManaColors.White, Choice = EffectOption.White},
        new Rel {Color = ManaColors.Blue, Choice = EffectOption.Blue},
        new Rel {Color = ManaColors.Black, Choice = EffectOption.Black},
        new Rel {Color = ManaColors.Red, Choice = EffectOption.Red},
        new Rel {Color = ManaColors.Green, Choice = EffectOption.Green},
      };

    public override ChosenOptions ChooseOptions()
    {
      var available = Target.Player().GetAvailableMana();

      var color = available.GetMostCommonColor();

      if (color == null)
        return new ChosenOptions(EffectOption.Green);

      return new ChosenOptions(Map.Single(x => x.Color == color).Choice);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var color = Map.Single(x => x.Choice.Equals(results.Options[0])).Color;

      var cardsToDiscard = Target.Player().Hand.Where(
        x => x.HasColors(color)).ToList();

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
      public ManaColors Color;
    }
  }
}