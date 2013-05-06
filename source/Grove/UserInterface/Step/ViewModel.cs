namespace Grove.UserInterface.Step
{
  using System.Linq;
  using Gameplay.Messages;
  using Gameplay.States;
  using Infrastructure;

  public class ViewModel : IReceive<StepStarted>
  {
    private readonly Configuration _configuration;
    private readonly Step[] _steps;

    public ViewModel(Step[] steps, string displayName, Configuration configuration)
    {
      _steps = steps;
      _configuration = configuration;
      DisplayName = displayName;
    }

    public Pass AutoPass { get { return _configuration.GetAutoPassConfiguration(_steps[0]); } }

    public string DisplayName { get; set; }
    public virtual bool IsCurent { get; set; }

    public void Receive(StepStarted message)
    {
      if (IsCurent)
        IsCurent = false;

      if (_steps.Any(x => x == message.Step))
      {
        IsCurent = true;
      }
    }

    [Updates("AutoPass")]
    public virtual void Toggle()
    {
      foreach (var step in _steps)
      {
        _configuration.ToggleAutoPass(step);
      }
    }

    public interface IFactory
    {
      // multiple steps can have one visual representation
      // this is done primarily to joint first strike combat damage
      // and normal combat damage steps in the ui.
      ViewModel Create(string displayName, params Step[] steps);
    }
  }
}