namespace Grove.Core
{
  using System.Linq;
  using Infrastructure;

  [Copyable]
  public class AssignedDamage : IHashable
  {
    private readonly TrackableList<Damage> _assigned;
    private readonly Player _player;

    private AssignedDamage() {}

    public AssignedDamage(Player player, ChangeTracker changeTracker)
    {
      _player = player;
      _assigned = new TrackableList<Damage>(changeTracker);
    }


    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_assigned);
    }

    public void Assign(Damage damage)
    {
      _assigned.Add(damage);
    }

    public void Deal()
    {
      foreach (Damage damage in _assigned)
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