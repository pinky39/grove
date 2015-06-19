namespace Grove
{
  using System.Linq;
  using Grove.Infrastructure;

  public class PreventLifelossBelowOneToPlayer : DamagePrevention
  {
    private readonly Player _player;

    public PreventLifelossBelowOneToPlayer(Player player)
    {
      _player = player;
    }

    private PreventLifelossBelowOneToPlayer() {}

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        calc.Calculate(_player));
    }

    public override int PreventLifeloss(int amount, Player player, bool queryOnly)
    {
      if (_player != player)
        return 0;

      var dealt = amount - CalculateLifeloss(amount);
      return dealt > 0 ? dealt : 0;
    }

    private int CalculateLifeloss(int lifeloss)
    {
      var controlsCreature = _player.Battlefield.Creatures.Any();

      if (!controlsCreature)
        return lifeloss;

      var lifeAfterDamage = _player.Life - lifeloss;

      if (lifeAfterDamage < 1)
      {
        return _player.Life - 1;
      }

      return lifeloss;
    }
  }
}