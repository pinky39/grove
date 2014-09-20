namespace Grove.Events
{
  public class StepStartedEvent
  {
    public readonly Step Step;

    public StepStartedEvent(Step step)
    {
      Step = step;
    }
  }
}