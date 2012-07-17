namespace Grove.Core.Details.Cards.Preventions
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class DamagePreventions : IModifiable, IHashable
  {
    private readonly TrackableList<DamagePrevention> _receivedPreventions;
    private readonly TrackableList<DamagePrevention> _dealtPreventions;
    
    private DamagePreventions() {}

    public DamagePreventions(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _receivedPreventions = new TrackableList<DamagePrevention>(changeTracker, hashDependancy);
      _dealtPreventions = new TrackableList<DamagePrevention>(changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_receivedPreventions);
    }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AddReceivedPrevention(DamagePrevention prevention)
    {
      _receivedPreventions.Add(prevention);
    }

    public void AddDealtPrevention(DamagePrevention prevention)
    {
      _dealtPreventions.Add(prevention);
    }

    public void PreventReceivedDamage(Damage damage)
    {
      foreach (var preventionEffect in _receivedPreventions.ToList())
      {
        preventionEffect.PreventDamage(damage);

        if (damage.Amount == 0)
          break;
      }
    }

    public void PreventDealtDamage(Damage damage)
    {
      foreach (var preventionEffect in _dealtPreventions.ToList())
      {
        preventionEffect.PreventDamage(damage);

        if (damage.Amount == 0)
          break;
      }
    }

    public int PreventLifeloss(int lifeloss)
    {
      foreach (var preventionEffect in _receivedPreventions.ToList())
      {
        lifeloss = preventionEffect.PreventLifeloss(lifeloss);

        if (lifeloss == 0)
          break;
      }

      return lifeloss;
    }

    public void Remove(DamagePrevention preventaion)
    {
      _receivedPreventions.Remove(preventaion);
    }

    public int EvaluateHowMuchDamageCanBeDealt(Card source, int amount, bool isCombat)
    {
      foreach (var preventionEffect in _receivedPreventions.ToList())
      {
        amount = preventionEffect.EvaluateHowMuchDamageCanBeDealt(source, amount, isCombat);

        if (amount == 0)
          break;
      }

      return amount;
    }
  }
}