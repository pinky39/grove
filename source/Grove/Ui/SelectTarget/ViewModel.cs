namespace Grove.Ui.SelectTarget
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Core;
  using Core.Targeting;
  using Infrastructure;

  public class ViewModel : ViewModelBase, IReceive<SelectionChanged>
  {
    private readonly bool _canCancel;    
    private readonly BindableCollection<ITarget> _selection = new BindableCollection<ITarget>();
    private readonly Action<ITarget> _targetSelected;
    private readonly Action<ITarget> _targetUnselected;
    private readonly object _triggerMessage;

    public ViewModel(SelectTargetParameters p)
    {
      TargetValidator = p.Validator;
      Instructions = p.Instructions;      
      _canCancel = p.CanCancel;
      _targetSelected = p.TargetSelected ?? DefaultTargetSelected;
      _targetUnselected = p.TargetUnselected ?? DefaultTargetUnselected;
      _triggerMessage = p.TriggerMessage;
    }

    private bool CanAutoComplete
    {
      get
      {
        return TargetValidator.MaxCount.HasValue &&
          _selection.Count == TargetValidator.MaxCount;
      }
    }

    public string Text { get { return TargetValidator.GetMessage(_selection.Count + 1); } }
    public string Instructions { get; private set; }
    private bool IsDone { get { return _selection.Count >= TargetValidator.MinCount; } }
    public IList<ITarget> Selection { get { return _selection; } }
    public TargetValidator TargetValidator { get; private set; }
    public bool WasCanceled { get; private set; }

    [Updates("Text")]
    public virtual void Receive(SelectionChanged message)
    {
      if (_selection.Count >= TargetValidator.MaxCount)
        return;

      if (_selection.Contains(message.Selection))
      {
        _targetUnselected(message.Selection);
        _selection.Remove(message.Selection);
        return;
      }

      if (TargetValidator.HasValidZone(message.Selection) == false)
        return;

      if (TargetValidator.IsTargetValid(message.Selection, _triggerMessage) == false)
        return;

      _targetSelected(message.Selection);
      _selection.Add(message.Selection);

      if (CanAutoComplete)
        Done();
    }

    private void DefaultTargetSelected(ITarget target)
    {
      Publish(new TargetSelected {Target = target});
    }

    private void DefaultTargetUnselected(ITarget target)
    {
      Publish(new TargetUnselected {Target = target});
    }

    public void Cancel()
    {
      if (!_canCancel)
        return;

      _selection.Clear();
      WasCanceled = true;
      this.Close();
    }

    public void Done()
    {
      if (!IsDone)
        return;

      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(SelectTargetParameters p);
    }
  }
}