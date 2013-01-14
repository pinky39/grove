namespace Grove.Core.Decisions
{
  using System;
  using System.Collections.Generic;
  using Effects;
  using Results;

  public abstract class ChooseEffectOptions : Decision<ChosenOptions>
  {
    public Func<ChooseEffectOptions, ChosenOptions> Ai;
    public Action<ChooseEffectOptions> EvaluateResults;
    public string Text { get; set; }
    public List<EffectChoice> Choices { get; set; }

    protected override bool ShouldExecuteQuery { get { return true; } }
    public Effect Effect { get; set; }

    public override void ProcessResults()
    {
      EvaluateResults(this);
    }
  }
}