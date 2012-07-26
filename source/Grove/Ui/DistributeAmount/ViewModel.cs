namespace Grove.Ui.DistributeAmount
{
  using System.Collections.Generic;
  using Core.Targeting;
  using Infrastructure;

  public class ViewModel
  {
    private readonly List<TargetWithValue> _targets = new List<TargetWithValue>();
    private readonly string _title;
    private readonly int _totalAmount;
    private int _toBeAssigned;

    public ViewModel(IEnumerable<ITarget> targets, int totalAmount, string title)
    {
      _totalAmount = totalAmount;
      _toBeAssigned = totalAmount;
      _title = title;
      WasCanceled = true;

      foreach (var target in targets)
      {
        _targets.Add(Bindable.Create<TargetWithValue>(target));
      }
    }

    public string Title { get { return string.Format("{0} ({1} left)", _title, _toBeAssigned); } }
    public bool WasCanceled { get; private set; }

    public IEnumerable<TargetWithValue> Targets { get { return _targets; } }
    public bool CanAccept { get { return _toBeAssigned == 0; } }

    [Updates("Title", "CanAccept")]
    public void AssignOne(TargetWithValue target)
    {
      target.Value++;
      _toBeAssigned--;
    }

    [Updates("Title", "CanAccept")]
    public void Clear()
    {
      _toBeAssigned = _totalAmount;

      foreach (var target in _targets)
      {
        target.Value = 0;
      }
    }


    public void Cancel()
    {
      this.Close();
    }

    public void Accept()
    {
      WasCanceled = false;
      this.Close();
    }

    public class TargetWithValue
    {
      public TargetWithValue(ITarget target)
      {
        Target = target;
      }

      public ITarget Target { get; private set; }
      public virtual int Value { get; set; }
    }
  }
}