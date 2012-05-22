namespace Grove.Core.Triggers
{
  using Infrastructure;
  using Messages;
  using Zones;

  public class AtBegginingOfStep : Trigger, IReceive<StepStarted>
  {
    public bool AtEach { get; set; }
    public Step Step { get; set; }

    public void Receive(StepStarted message)
    {
      if (message.Step != Step || Ability.OwningCard.Zone != Zone.Battlefield) 
        return;
      
      if (AtEach || Ability.OwningCard.Controller.IsActive)
        Set();
    }
  }
}