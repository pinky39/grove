namespace Grove.Core.Cards.Effects
{
  using System;
  using System.Collections.Generic;
  using Grove.Core.Decisions;
  using Grove.Core.Decisions.Results;

  public class CustomizableEffect : Effect
  {
    private readonly List<EffectChoice> _choices = new List<EffectChoice>();
    public Func<ChooseEffectOptions, ChosenOptions> Ai;
    public Action<ChooseEffectOptions> ProcessResults;
    public string Text;

    public void Choices(params EffectChoice[] choices)
    {
      _choices.AddRange(choices);
    }

    protected override void ResolveEffect()
    {
      Game.Enqueue<ChooseEffectOptions>(Controller, p =>
        {
          p.Ai = Ai;
          p.EvaluateResults = ProcessResults;
          p.Text = Text;
          p.Choices = _choices;
          p.Effect = this;
        });      
    }
  }
}