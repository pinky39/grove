namespace Grove.Ui.SelectTarget
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Core.Targeting;
  using Infrastructure;

  public class ViewModel : IReceive<TargetSelected>
  {
    private readonly bool _canCancel;
    private readonly BindableCollection<ITarget> _selection = new BindableCollection<ITarget>();
    private readonly Action<ITarget> _targetSelected;
    private readonly Action<ITarget> _targetUnselected;

    public ViewModel(ITargetValidator targetValidator, bool canCancel) : this(targetValidator, canCancel, null) {}

    public ViewModel(ITargetValidator targetValidator, bool canCancel, string instructions)
      : this(targetValidator, canCancel, instructions, null, null) {}

    public ViewModel(ITargetValidator targetValidator, bool canCancel, string instructions,
      Action<ITarget> targetSelected, Action<ITarget> targetUnselected)
    {
      TargetValidator = targetValidator;
      Instructions = instructions;
      _canCancel = canCancel;
      _targetUnselected = targetUnselected ?? delegate { };
      _targetSelected = targetSelected ?? delegate { };
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


    [Updates("Text")]
    public virtual void Receive(TargetSelected message)
    {
      if (_selection.Count >= TargetValidator.MaxCount)
        return;

      if (_selection.Contains(message.Target))
      {
        _targetUnselected(message.Target);
        _selection.Remove(message.Target);
        return;
      }

      if (!TargetValidator.IsValid(message.Target))
        return;

      _targetSelected(message.Target);
      _selection.Add(message.Target);

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