namespace Grove.Gameplay.Modifiers
{
  using System;
  using Infrastructure;
  using Messages;

  public class EndOfStep : Lifetime, IReceive<StepFinished>
  {
    private readonly Step _step;
    private readonly Func<EndOfStep, bool> _filter;

    private EndOfStep() {}

    public EndOfStep(Step step, Func<EndOfStep, bool> filter = null)
    {
      _step = step;
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(StepFinished message)
    {
      if (message.Step == _step)
      {
        if (_filter != null && _filter(this))
        {
          return;
        }

        End();
      }
    }
  }
}