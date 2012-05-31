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

    public void Assign(Card source, int amount)
    {
      if (amount <= 0)
        return;

      _assigned.Add(new Damage(source, amount));
    }

    public void Deal()
    {
      foreach (var damage in _assigned)
      {
        _player.DealDamage(damage.Source, damage.Amount, isCombat: true);
      }
      _assigned.Clear();
    }

    public override string ToString()
    {
      return _assigned.Sum(x => x.Amount).ToString();
    }
  }
}