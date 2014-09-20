namespace Grove.Modifiers
{
  using System;
  using Grove.Events;
  using Grove.Infrastructure;

  public class EndOfStep : Lifetime, IReceive<StepFinishedEvent>
  {
    private readonly Step _step;
    private readonly Func<EndOfStep, bool> _filter;

    private EndOfStep() {}

    public EndOfStep(Step step, Func<EndOfStep, bool> filter = null)
    {
      _step = step;
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(StepFinishedEvent message)
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