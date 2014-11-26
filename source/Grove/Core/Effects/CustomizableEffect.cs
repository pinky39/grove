namespace Grove.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

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

    protected static readonly List<ChoiceToZone> ChoiceToZoneMap = new List<ChoiceToZone>
      {
        new ChoiceToZone() {Zone = Zone.Library, Choice = EffectOption.Library},
        new ChoiceToZone() {Zone = Zone.Graveyard, Choice = EffectOption.Graveyard},
        new ChoiceToZone() {Zone = Zone.Hand, Choice = EffectOption.Hand},
      };

    public abstract ChosenOptions ChooseResult(List<IEffectChoice> candidates);
    public abstract void ProcessResults(ChosenOptions results);
    public abstract string GetText();
    public abstract IEnumerable<IEffectChoice> GetChoices();
    protected virtual Player SelectChoosingPlayer()
    {
      return Controller;
    }

    protected override void ResolveEffect()
    {
      Enqueue(new ChooseEffectOptions(SelectChoosingPlayer(), p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Text = GetText();
          p.Choices = GetChoices().ToList();
        }));
    }

    protected class ChoiceToColor
    {
      public EffectOption Choice;
      public CardColor Color;
    }

    protected class ChoiceToZone
    {
      public EffectOption Choice;
      public Zone Zone;
    }
  }
}