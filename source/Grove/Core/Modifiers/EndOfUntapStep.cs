namespace Grove.Core.Modifiers
{
  using System;
  using Infrastructure;
  using Messages;

  public class EndOfUntapStep : Lifetime, IReceive<StepFinished>
  {
    private readonly Func<EndOfUntapStep, bool> _filter;

    private EndOfUntapStep() {}

    public EndOfUntapStep(Func<EndOfUntapStep, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    public void Receive(StepFinished message)
    {
      if (message.Step == Step.Untap)
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