namespace Grove.Ui.Step
{
  using Core;
  using Core.Messages;
  using Infrastructure;

  public class ViewModel : IReceive<StepStarted>
  {
    private readonly Configuration _configuration;

    public ViewModel(Step step, string displayName, Configuration configuration)
    {
      _configuration = configuration;
      Step = step;
      DisplayName = displayName;
    }

    public Pass AutoPass { get { return _configuration.GetAutoPassConfiguration(Step); } }

    public string DisplayName { get; set; }
    public virtual bool IsCurent { get; set; }
    public Step Step { get; set; }

    public void Receive(StepStarted message)
    {
      if (IsCurent)
        IsCurent = false;

      if (message.Step == Step)
      {
        IsCurent = true;
      }
    }

    [Updates("AutoPass")]
    public virtual void Toggle()
    {
      _configuration.ToggleAutoPass(Step);
    }

    public interface IFactory
    {
      ViewModel Create(Step step, string displayName);
    }
  }
}