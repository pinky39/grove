namespace Grove.Gameplay.Damage
{
  using System;
  using Infrastructure;

  [Copyable, Serializable]
  public class Damage : IHashable
  {
    private readonly Trackable<int> _amount;
    private readonly TrackableList<DamageRedirection> _redirections;

    private Damage() {}

    public Damage(Card source, int amount, bool isCombat, ChangeTracker changeTracker)
    {
      Source = source;
      IsCombat = isCombat;

      _amount = new Trackable<int>(amount).Initialize(changeTracker);
      _redirections = new TrackableList<DamageRedirection>().Initialize(changeTracker);
    }

    public int Amount { get { return _amount.Value; } set { _amount.Value = value; } }
    public bool IsCombat { get; private set; }

    public bool IsLeathal { get { return Source.Has().Deathtouch; } }
    public Card Source { get; private set; }

    public int CalculateHash(HashCalculator calc)
    {
      return Amount;
    }

    public void PreventAll()
    {
      Amount = 0;
    }

    public int Prevent(int amount)
    {
      if (amount > Amount)
        amount = Amount;

      Amount = Amount - amount;

      return amount;
    }

    public bool WasAlreadyRedirected(DamageRedirection damageRedirection)
    {
      return _redirections.Contains(damageRedirection);
    }

    public void AddRedirection(DamageRedirection damageRedirection)
    {
      _redirections.Add(damageRedirection);
    }
  }
}