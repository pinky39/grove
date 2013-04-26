namespace Grove.Gameplay.Messages
{
  using States;

  public class StepStarted
  {
    public StepStarted(Step step)
    {
      Step = step;
    }

    public Step Step { get; private set; }
  }
}