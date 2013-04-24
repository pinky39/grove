namespace Grove.Ui.SelectTarget
{
  using System;
  using Gameplay.Targeting;

  public class SelectTargetParameters
  {
    public bool CanCancel;
    public string Instructions;
    public Action<ITarget> TargetSelected;
    public Action<ITarget> TargetUnselected;
    public object TriggerMessage;
    public TargetValidator Validator;
    public int? X;
  }
}