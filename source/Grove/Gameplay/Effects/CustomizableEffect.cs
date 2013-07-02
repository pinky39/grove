namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Characteristics;
  using Decisions;
  using Decisions.Results;

  public abstract class CustomizableEffect : Effect, IProcessDecisionResults<ChosenOptions>,
    IChooseDecisionResults<List<IEffectChoice>, ChosenOptions>
  {
    protected static readonly List<ChoiceToColor> ChoiceToColorMap = new List<ChoiceToColor>
      {
        new ChoiceToColor {Color = CardColor.White, Choice = EffectOption.White},
        new ChoiceToColor {Color = CardColor.Blue, Choice = EffectOption.Blue},
        new ChoiceToColor {Color = CardColor.Black, Choice = EffectOption.Black},
        new ChoiceToColor {Color = CardColor.Red, Choice = EffectOption.Red},
        new ChoiceToColor {Color = CardColor.Green, Choice = EffectOption.Green},
      };

    public abstract ChosenOptions ChooseResult(List<IEffectChoice> candidates);
    public abstract void ProcessResults(ChosenOptions results);
    public abstract string GetText();
    public abstract IEnumerable<IEffectChoice> GetChoices();

    protected override void ResolveEffect()
    {
      Enqueue<ChooseEffectOptions>(Controller, p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Text = GetText();
          p.Choices = GetChoices().ToList();
        });
    }

    protected class ChoiceToColor
    {
      public EffectOption Choice;
      public CardColor Color;
    }
  }
}