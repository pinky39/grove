namespace Grove.Gameplay
{
  using System.Linq;
  using Grove.Infrastructure;

  [Copyable]
  public class AssignedDamage : IHashable
  {
    private readonly TrackableList<DamageFromSource> _assigned = new TrackableList<DamageFromSource>();
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

    public void Assign(DamageFromSource damage)
    {
      _assigned.Add(damage);
    }

    public void Deal()
    {
      foreach (var damage in _assigned)
      {
        damage.Source.DealDamageTo(damage.Amount, _player, isCombat: true);
      }
      _assigned.Clear();
    }

    public override string ToString()
    {
      return _assigned.Sum(x => x.Amount).ToString();
    }
  }
}