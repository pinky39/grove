namespace Grove.Gameplay.Damage
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;
  using Targeting;

  [Copyable]
  public class DamagePreventions : IModifiable, IHashable
  {
    private readonly TrackableList<DamagePrevention> _preventions = new TrackableList<DamagePrevention>();
    public int Count { get { return _preventions.Count; } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_preventions);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ITarget owner, Game game, IHashDependancy hashDependancy = null)
    {
      _preventions.Initialize(game.ChangeTracker, hashDependancy);

      foreach (var damagePrevention in _preventions)
      {
        damagePrevention.Initialize(owner, game);
      }
    }

    public void AddPrevention(DamagePrevention prevention)
    {
      _preventions.Add(prevention);
    }

    public void PreventReceivedDamage(Damage damage)
    {
      foreach (var preventionEffect in _preventions.ToList())
      {
        preventionEffect.PreventReceivedDamage(damage);

        if (damage.Amount == 0)
          break;
      }
    }

    public int PreventDealtCombatDamage(int amount)
    {
      foreach (var preventionEffect in _preventions.ToList())
      {
        amount = preventionEffect.PreventDealtCombatDamage(amount);

        if (amount == 0)
          break;
      }

      return amount;
    }

    public int PreventLifeloss(int lifeloss)
    {
      foreach (var preventionEffect in _preventions.ToList())
      {
        lifeloss = preventionEffect.PreventLifeloss(lifeloss);

        if (lifeloss == 0)
          break;
      }

      return lifeloss;
    }

    public void Remove(DamagePrevention preventaion)
    {
      _preventions.Remove(preventaion);
    }

    public int EvaluateReceivedDamage(Card source, int amount, bool isCombat)
    {
      foreach (var preventionEffect in _preventions.ToList())
      {
        amount = preventionEffect.EvaluateReceivedDamage(source, amount, isCombat);

        if (amount == 0)
          break;
      }

      return amount;
    }

    public int EvaluateDealtCombatDamage(int amount)
    {
      foreach (var preventionEffect in _preventions.ToList())
      {
        amount = preventionEffect.EvaluateDealtCombatDamage(amount);

        if (amount == 0)
          break;
      }

      return amount;
    }
  }
}