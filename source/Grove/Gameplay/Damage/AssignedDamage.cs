namespace Grove.Gameplay.Damage
{
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class AssignedDamage : IHashable
  {
    private readonly TrackableList<Damage> _assigned = new TrackableList<Damage>();
    private readonly Player _player;

    private AssignedDamage() {}

    public AssignedDamage(Player player)
    {
      _player = player;
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_assigned);
    }

    public void Initialize(ChangeTracker changeTracker)
    {
      _assigned.Initialize(changeTracker);
    }

    public void Assign(Damage damage)
    {
      _assigned.Add(damage);
    }

    public void Deal()
    {
      foreach (var damage in _assigned)
      {
        _player.DealDamage(damage);
      }
      _assigned.Clear();
    }

    public override string ToString()
    {
      return _assigned.Sum(x => x.Amount).ToString();
    }
  }
}