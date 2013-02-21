namespace Grove.Ui.SelectTarget
{
  using System;
  using Core;
  using Core.Targeting;

  public class SelectTargetParameters
  {
    public Card OwningCard;
    public object TriggerMessage;
    public TargetValidator Validator;
    public bool CanCancel;
    public string Instructions;
    public Action<ITarget> TargetSelected;
    public Action<ITarget> TargetUnselected;
  }
}