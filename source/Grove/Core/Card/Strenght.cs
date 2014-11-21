namespace Grove
{
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Strenght : IAcceptsCardModifier, ICopyContributor, IHashable
  {
    private readonly CardBase _cardBase;
    private readonly Characteristic<int?> _power;
    private readonly Trackable<bool> _switchPowerAndToughness = new Trackable<bool>();
    private readonly Characteristic<int?> _toughness;

    private Strenght() {}

    public Strenght(CardBase cardBase)
    {
      _cardBase = cardBase;

      _power = new Characteristic<int?>(cardBase.Value.Power);
      _toughness = new Characteristic<int?>(cardBase.Value.Toughness);
    }

    public int? Power
    {
      get
      {
        var characteristic = _switchPowerAndToughness ? _toughness : _power;
        return characteristic.Value < 0 ? 0 : characteristic.Value;
      }
    }

    public int? Toughness
    {
      get
      {
        var characteristic = _switchPowerAndToughness ? _power : _toughness;
        return characteristic.Value < 0 ? 0 : characteristic.Value;
      }
    }

    public int? BaseToughness
    {
      get { return _toughness.BaseValue; }
    }

    public int? BasePower
    {
      get { return _power.BaseValue; }
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
    }

    public void Initialize(Game game, IHashDependancy hashDependancy)
    {
      _power.Initialize(game, hashDependancy);
      _toughness.Initialize(game, hashDependancy);
      _switchPowerAndToughness.Initialize(game.ChangeTracker, hashDependancy);

      _cardBase.Changed += OnCardBaseChanged;
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

    public void SwitchPowerAndToughness()
    {
      _switchPowerAndToughness.Value = !_switchPowerAndToughness.Value;
    }

    private void OnCardBaseChanged()
    {
      _power.ChangeBaseValue(_cardBase.Value.Power);
      _toughness.ChangeBaseValue(_cardBase.Value.Toughness);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        _power.Value.GetHashCode(),
        _toughness.Value.GetHashCode());
    }
  }
}