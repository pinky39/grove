namespace Grove.Gameplay.Modifiers
{
  using System;
  using Grove.Infrastructure;
  using Messages;
  using States;

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