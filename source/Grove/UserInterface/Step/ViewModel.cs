﻿namespace Grove.UserInterface.Step
{
  using System.Linq;
  using Events;
  using Infrastructure;
  using Step = Grove.Step;

  public class ViewModel : IReceive<StepStartedEvent>
  {    
    private readonly Step[] _steps;

    public ViewModel(Step[] steps, string displayName)
    {
      _steps = steps;      
      DisplayName = displayName;
    }

    public Pass AutoPass { get { return Ui.Configuration.GetAutoPassConfiguration(_steps[0]); } }

    public string DisplayName { get; set; }
    public virtual bool IsCurent { get; set; }

    public void Receive(StepStartedEvent message)
    {
      if (IsCurent)
        IsCurent = false;

      if (IsStep(message.Step))
      {
        IsCurent = true;
      }
    }

    public bool IsStep(Step step)
    {
      return _steps.Any(x => x == step);
    }

    [Updates("AutoPass")]
    public virtual void Toggle()
    {
      foreach (var step in _steps)
      {
        Ui.Configuration.ToggleAutoPass(step);
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