namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Decisions;
  using Modifiers;

  public class TargetGainsProtectionFromChosenColor : CustomizableEffect
  {
    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      CardColor? color = null;

      if (!Stack.IsEmpty && !Stack.TopSpell.HasColor(CardColor.Colorless) && 
          !Stack.TopSpell.HasColor(CardColor.None))
      {
        color = Stack.TopSpell.Colors[0];
      }

      color = color ?? GetMostCommonColor(Controller.Opponent.Battlefield);

      return new ChosenOptions(ChoiceToColorMap.Single(x => x.Color == color).Choice);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      var color = ChoiceToColorMap.Single(x => x.Choice.Equals(results.Options[0])).Color;

      var modifier = new AddProtectionFromColors(color) {UntilEot = true};

      var parameters = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard
        };

      Target.Card().AddModifier(modifier, parameters);
    }

    public override string GetText()
    {
      return "Target gains protection from #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.White,
        EffectOption.Blue,
        EffectOption.Black,
        EffectOption.Red,
        EffectOption.Green);
    }
  }
}