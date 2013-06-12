namespace Grove.Gameplay.DamageHandling
{
  using System;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class DamagePreventions : IAcceptsGameModifier, IHashable
  {
    private readonly TrackableList<DamagePrevention> _preventions = new TrackableList<DamagePrevention>();
    public int Count { get { return _preventions.Count; } }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_preventions);
    }

    public void Accept(IGameModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(INotifyChangeTracker changeTracker)
    {
      _preventions.Initialize(changeTracker);
    }

    public void AddPrevention(DamagePrevention prevention)
    {
      _preventions.Add(prevention);
    }

    public int PreventDamage(PreventDamageParameters preventDamageParameters)
    {
      return Prevent(preventDamageParameters.Amount, (prevention) => prevention.PreventDamage(preventDamageParameters));
    }

    public int PreventLifeloss(int amount, Player player, bool queryOnly = true)
    {
      return Prevent(amount, (prevention) => prevention.PreventLifeloss(amount, player, queryOnly));
    }

    private int Prevent(int amount, Func<DamagePrevention, int> getPreventedDamage)
    {
      var totalPrevented = 0;

      foreach (var prevention in _preventions)
      {
        totalPrevented += getPreventedDamage(prevention);

        if (totalPrevented >= amount)
          break;
      }

      return totalPrevented > amount ? amount : totalPrevented;
    }

    public void Remove(DamagePrevention preventaion)
    {
      _preventions.Remove(preventaion);
    }
  }
}