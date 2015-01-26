namespace Grove.Effects
{
  using System;
  using Decisions;
  using Modifiers;

  public class ChooseOneEffectFromGiven : Effect, IChooseDecisionResults<BooleanResult>,
    IProcessDecisionResults<BooleanResult>
  {
    private readonly Effect _ifTrueEffect;
    private readonly Effect _ifFalseEffect;
    private readonly Func<Effect, Game, bool> _chooseAi;
    private readonly string _message;

    private ChooseOneEffectFromGiven() { }

    public ChooseOneEffectFromGiven(string message, Effect ifTrueEffect, Effect ifFalseEffect, Func<Effect, Game, bool> chooseAi)
    {
      _chooseAi = chooseAi;
      _ifTrueEffect = ifTrueEffect;
      _ifFalseEffect = ifFalseEffect;
      _message = message;
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      _ifTrueEffect.Initialize(p, game, evaluateParameters);
      _ifFalseEffect.Initialize(p, game, evaluateParameters);

      return this;
    }

    public override void SetTriggeredAbilityTargets(Targets targets)
    {
      _ifTrueEffect.SetTriggeredAbilityTargets(targets);
      _ifFalseEffect.SetTriggeredAbilityTargets(targets);
    }

    protected override void ResolveEffect()
    {
      Enqueue(new ChooseTo(Controller, p =>
      {
        p.Text = _message;
        p.ProcessDecisionResults = this;
        p.ChooseDecisionResults = this;
      }));
    }

    public BooleanResult ChooseResult()
    {
      return _chooseAi(this, Game);
    }

    public void ProcessResults(BooleanResult results)
    {
      var effect = results.IsTrue ? _ifTrueEffect : _ifFalseEffect;

      effect.BeginResolve();
    }
  }
}
