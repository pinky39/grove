namespace Grove.Events
{
  public class StepFinishedEvent
  {
    public readonly Step Step;

    public StepFinishedEvent(Step step)
    {
      Step = step;
    }
  }
}