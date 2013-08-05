namespace Grove.Gameplay.Characteristics
{
  using System;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Strenght : IAcceptsCardModifier
  {
    private readonly Characteristic<int?> _power;
    private readonly Characteristic<int?> _toughness;
    private IHashDependancy _hashDependancy;

    private Strenght() {}

    public Strenght(int? power, int? toughness)
    {
      _power = new Characteristic<int?>(power);
      _toughness = new Characteristic<int?>(toughness);
    }

    public int? Power { get { return _power.Value < 0 ? 0 : _power.Value; } }
    public int? Toughness { get { return _toughness.Value < 0 ? 0 : _toughness.Value; } }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(Game game, IHashDependancy hashDependancy)
    {
      _power.Initialize(game, hashDependancy);
      _toughness.Initialize(game, hashDependancy);
      _hashDependancy = hashDependancy;
    }

    public void AddPowerModifier(PropertyModifier<int?> modifier)
    {
      _power.AddModifier(modifier);
    }

    public void AddToughnessModifier(PropertyModifier<int?> modifier)
    {
      _toughness.AddModifier(modifier);
    }

    public void RemovePowerModifier(PropertyModifier<int?> modifier)
    {
      _power.RemoveModifier(modifier);
    }

    public void RemoveToughnessModifier(PropertyModifier<int?> modifier)
    {
      _toughness.RemoveModifier(modifier);
    }
  }
}