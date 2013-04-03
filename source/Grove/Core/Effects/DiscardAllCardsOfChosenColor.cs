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
        new Rel {Color = ManaColors.White, Choice = EffectChoiceOption.White},
        new Rel {Color = ManaColors.Blue, Choice = EffectChoiceOption.Blue},
        new Rel {Color = ManaColors.Black, Choice = EffectChoiceOption.Black},
        new Rel {Color = ManaColors.Red, Choice = EffectChoiceOption.Red},
        new Rel {Color = ManaColors.Green, Choice = EffectChoiceOption.Green},
      };

    public override ChosenOptions ChooseOptions()
    {
      var available = Target.Player().GetAvailableMana();

      var color = available.GetMostCommonColor();

      if (color == null)
        return new ChosenOptions(EffectChoiceOption.Green);

      return new ChosenOptions(Map.Single(x => x.Color == color).Choice);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var color = Map.Single(x => x.Choice == results.Options[0]).Color;

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

    public override IEnumerable<EffectChoice> GetChoices()
    {
      yield return new EffectChoice(
        EffectChoiceOption.White,
        EffectChoiceOption.Blue,
        EffectChoiceOption.Black,
        EffectChoiceOption.Red,
        EffectChoiceOption.Green);
    }

    private class Rel
    {
      public EffectChoiceOption Choice;
      public ManaColors Color;
    }
  }
}