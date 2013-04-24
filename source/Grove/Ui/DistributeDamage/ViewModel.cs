namespace Grove.Ui.DistributeDamage
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay;
  using Gameplay.Card;
  using Gameplay.Targeting;
  using Infrastructure;

  public class ViewModel
  {
    private readonly int _amount;
    private readonly Game _game;
    private readonly List<TargetWithValue> _targets = new List<TargetWithValue>();
    private int _toBeAssigned;    

    public ViewModel(IEnumerable<ITarget> targets, int amount, Game game)
    {
      _amount = amount;
      _game = game;
      _toBeAssigned = amount;

      foreach (var target in targets)
      {
        _targets.Add(Bindable.Create<TargetWithValue>(target));
      }
    }

    public string Title { get { return string.Format("Distribute damage ({0} left)", _toBeAssigned); } }
    public IEnumerable<TargetWithValue> Targets { get { return _targets; } }
    public bool CanAccept { get { return _toBeAssigned == 0; } }
    public List<int> Distribution { get { return _targets.Select(x => x.Damage).ToList(); } }

    [Updates("Title", "CanAccept")]
    public virtual void AssignOne(TargetWithValue target)
    {
      if (_toBeAssigned == 0)
        return;
      
      target.Damage++;
      _toBeAssigned--;
    }

    [Updates("Title", "CanAccept")]
    public virtual void Clear()
    {
      _toBeAssigned = _amount;

      foreach (var target in _targets)
      {
        target.Damage = 0;
      }
    }

    public void Accept()
    {
      this.Close();
    }

    public void ChangePlayersInterest(Card card)
    {
      _game.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
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
      public virtual int Damage { get; set; }
    }
  }
}