namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;

  public abstract class CustomizableEffect : Effect, IProcessDecisionResults<ChosenOptions>, IChooseDecisionResults<List<object>, ChosenOptions>
  {
    public abstract ChosenOptions ChooseResult(List<object> candidates);
    public abstract void ProcessResults(ChosenOptions results);
    public abstract string GetText();
    public abstract IEnumerable<object> GetChoices();

    protected override void ResolveEffect()
    {
      Enqueue<ChooseEffectOptions>(Controller, p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Text = GetText();
          p.Choices = GetChoices().ToList();
          p.Effect = this;
        });
    }
  }
}