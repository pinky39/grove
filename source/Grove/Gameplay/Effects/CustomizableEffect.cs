namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Decisions.Results;

  public abstract class CustomizableEffect : Effect, IProcessDecisionResults<ChosenOptions>,
    IChooseDecisionResults<List<IEffectChoice>, ChosenOptions>
  {
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
  }
}