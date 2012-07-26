namespace Grove.Ui.DistributeDamage
{
  using System.Collections.Generic;
  using System.Linq;
  using Core.Targeting;
  using Infrastructure;

  public class ViewModel
  {
    private readonly int _amount;
    private readonly List<TargetWithValue> _targets = new List<TargetWithValue>();
    private int _toBeAssigned;

    public ViewModel(IEnumerable<ITarget> targets, int amount)
    {
      _amount = amount;
      _toBeAssigned = amount;

      foreach (var target in targets)
      {
        _targets.Add(Bindable.Create<TargetWithValue>(target));
      }
    }

    public string Title { get { return string.Format("Distribute damage ({0} left)", _toBeAssigned); } }
    public IEnumerable<TargetWithValue> Targets { get { return _targets; } }
    public bool CanAccept { get { return _toBeAssigned == 0; } }
    public IList<int> Distribution { get { return _targets.Select(x => x.Value).ToList(); } }

    [Updates("Title", "CanAccept")]
    public void AssignOne(TargetWithValue target)
    {
      target.Value++;
      _toBeAssigned--;
    }

    [Updates("Title", "CanAccept")]
    public void Clear()
    {
      _toBeAssigned = _amount;

      foreach (var target in _targets)
      {
        target.Value = 0;
      }
    }

    public void Accept()
    {
      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(IEnumerable<ITarget> targets, int amount);
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