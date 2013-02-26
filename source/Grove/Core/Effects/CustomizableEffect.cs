namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;

  public abstract class CustomizableEffect : Effect, IProcessDecisionResults<ChosenOptions>, IChooseEffectOptionsAi
  {
    public abstract ChosenOptions ChooseOptions();
    public abstract void ProcessResults(ChosenOptions results);
    public abstract string GetText();
    public abstract IEnumerable<EffectChoice> GetChoices();

    protected override void ResolveEffect()
    {
      Enqueue<ChooseEffectOptions>(Controller, p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseOptionsAi = this;
          p.Text = GetText();
          p.Choices = GetChoices().ToList();
          p.Effect = this;
        });
    }
  }
}