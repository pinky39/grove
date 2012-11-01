namespace Grove.Ui.SelectTarget
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Core.Targeting;
  using Infrastructure;

  public class ViewModel : IReceive<SelectionChanged>
  {
    private readonly bool _canCancel;
    private readonly Publisher _publisher;
    private readonly BindableCollection<ITarget> _selection = new BindableCollection<ITarget>();
    private readonly Action<ITarget> _targetSelected;
    private readonly Action<ITarget> _targetUnselected;

    public ViewModel(ITargetValidator targetValidator, bool canCancel, Publisher publisher) : this(targetValidator, canCancel, null, publisher) {}

    public ViewModel(ITargetValidator targetValidator, bool canCancel, string instructions, Publisher publisher)
      : this(targetValidator, canCancel, instructions, null, null, publisher) {}

    public ViewModel(ITargetValidator targetValidator, bool canCancel, string instructions,
      Action<ITarget> targetSelected, Action<ITarget> targetUnselected, Publisher publisher)
    {
      TargetValidator = targetValidator;
      Instructions = instructions;
      _canCancel = canCancel;
      _publisher = publisher;      
      _targetSelected = targetSelected ?? DefaultTargetSelected;
      _targetUnselected = targetUnselected ?? DefaultTargetUnselected;
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
    public ITargetValidator TargetValidator { get; private set; }
    public bool WasCanceled { get; private set; }    

    private void DefaultTargetSelected(ITarget target)
    {
      _publisher.Publish(new TargetSelected {Target = target});
    }

    private void DefaultTargetUnselected(ITarget target)
    {
      _publisher.Publish(new TargetUnselected {Target = target});
    }

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

      if (!TargetValidator.IsValid(message.Selection))
        return;

      _targetSelected(message.Selection);
      _selection.Add(message.Selection);

      if (CanAutoComplete)
        Done();
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
      ViewModel Create(
        ITargetValidator targetValidator,
        bool canCancel,
        string instructions = null,
        Action<ITarget> targetSelected = null,
        Action<ITarget> targetUnselected = null);
    }
  }
}