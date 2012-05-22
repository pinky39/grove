namespace Grove.Core
{
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  public interface IHasStaticAbilities
  {
    bool Deathtouch { get; }
    bool Defender { get; }
    bool Fear { get; }
    bool Flying { get; }
    bool Haste { get; }
    bool Hexproof { get; }
    bool Indestructible { get; }
    bool Lifelink { get; }
    bool Shroud { get; }
    bool Trample { get; }
    bool Unblockable { get; }
  }

  [Copyable]
  public class StaticAbilities : IHasStaticAbilities, IModifiable, IHashable
  {
    private readonly TrackableList<StaticAbility> _abilities;

    private StaticAbilities() {}

    public StaticAbilities(IEnumerable<StaticAbility> staticAbilities, ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _abilities = new TrackableList<StaticAbility>(staticAbilities, changeTracker, hashDependancy);
    }

    public int CalculateHash(HashCalculator hashCalculator)
    {
      return hashCalculator.Calculate(_abilities);
    }

    public bool Deathtouch { get { return Has(StaticAbility.Deathtouch); } }
    public bool Defender { get { return Has(StaticAbility.Defender); } }
    public bool Fear { get { return Has(StaticAbility.Fear); } }
    public bool Flying { get { return Has(StaticAbility.Flying); } }
    public bool Haste { get { return Has(StaticAbility.Haste); } }
    public bool Hexproof { get { return Has(StaticAbility.Hexproof); } }
    public bool Indestructible { get { return Has(StaticAbility.Indestructible); } }
    public bool Lifelink { get { return Has(StaticAbility.Lifelink); } }
    public bool Shroud { get { return Has(StaticAbility.Shroud); } }
    public bool Trample { get { return Has(StaticAbility.Trample); } }
    public bool Unblockable { get { return Has(StaticAbility.Unblockable); } }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Add(StaticAbility ability)
    {
      _abilities.Add(ability);
    }

    public void Remove(StaticAbility ability)
    {
      _abilities.Remove(ability);
    }

    private bool Has(StaticAbility ability)
    {
      return _abilities.Contains(ability);
    }
  }
}