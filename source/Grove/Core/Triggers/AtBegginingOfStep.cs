namespace Grove.Core.Triggers
{
  using Ai;
  using Infrastructure;
  using Messages;

  public class AtBegginingOfStep : Trigger, IReceive<StepStarted>
  {
    public bool ActiveTurn = true;
    public bool PassiveTurn = false;

    public Step Step { get; set; }

    public void Receive(StepStarted message)
    {
      if (message.Step != Step)
        return;

      if (ActiveTurn && Controller.IsActive || PassiveTurn && !Controller.IsActive)
      {
        Set();
      }
    }
  }
}