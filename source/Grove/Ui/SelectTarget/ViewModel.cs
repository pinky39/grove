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
    private readonly BindableCollection<Target> _selection = new BindableCollection<Target>();
    private readonly Action<Target> _targetSelected;
    private readonly Action<Target> _targetUnselected;

    public ViewModel(ITargetSelector targetSelector, bool canCancel) : this(targetSelector, canCancel, null) {}

    public ViewModel(ITargetSelector targetSelector, bool canCancel, string instructions)
      : this(targetSelector, canCancel, instructions, null, null) {}

    public ViewModel(ITargetSelector targetSelector, bool canCancel, string instructions,
                     Action<Target> targetSelected, Action<Target> targetUnselected)
    {
      TargetSelector = targetSelector;
      Instructions = instructions;
      _canCancel = canCancel;
      _targetUnselected = targetUnselected ?? delegate { };
      _targetSelected = targetSelected ?? delegate { };
    }

    private bool CanAutoComplete
    {
      get
      {
        return TargetSelector.MaxCount.HasValue &&
          _selection.Count == TargetSelector.MaxCount;
      }
    }

    public string Instructions { get; private set; }
    private bool IsDone { get { return _selection.Count >= TargetSelector.MinCount; } }
    public IList<Target> Selection { get { return _selection; } }
    public ITargetSelector TargetSelector { get; private set; }
    public bool WasCanceled { get; private set; }

    public virtual void Receive(TargetSelected message)
    {
      if (_selection.Count >= TargetSelector.MaxCount)
        return;

      if (_selection.Contains(message.Target))
      {
        _targetUnselected(message.Target);
        _selection.Remove(message.Target);
        return;
      }

      if (!TargetSelector.IsValid(message.Target))
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
        ITargetSelector targetSelector,
        bool canCancel,
        string instructions = null,
        Action<Target> targetSelected = null,
        Action<Target> targetUnselected = null);
    }
  }
}