namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;

  public class AtBegginingOfStep : Trigger, IReceive<StepStarted>
  {
    public bool AtEach { get; set; }
    public Step Step { get; set; }

    public void Receive(StepStarted message)
    {
      if (message.Step != Step)
        return;

      if (AtEach || Ability.OwningCard.Controller.IsActive)
        Set();
    }
  }
}