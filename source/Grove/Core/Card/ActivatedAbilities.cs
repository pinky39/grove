namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  public class ActivatedAbilities : GameObject, IAcceptsCardModifier, IHashable, ICopyContributor
  {
    private readonly Characteristic<List<ActivatedAbility>> _abilities;
    private readonly CardBase _cardBase;

    private ActivatedAbilities() {}

    public ActivatedAbilities(CardBase cardBase)
    {
      _cardBase = cardBase;      

      _abilities = new Characteristic<List<ActivatedAbility>>(cardBase.Value.ActivatedAbilities);      
    }

    private void OnAbilitiesChanged(CharacteristicChangedParams<List<ActivatedAbility>> p)
    {
      var abilitiesToDeactivate = p.OldValue.Where(x => !p.NewValue.Contains(x)).ToList();
      var abilitiesToActivate = p.NewValue.Where(x => !p.OldValue.Contains(x)).ToList();

      foreach (var ability in abilitiesToDeactivate)
      {
        ability.OnDisable();
      }

      foreach (var ability in abilitiesToActivate)
      {
        ability.OnEnable();
      }
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
      _abilities.Changed += OnAbilitiesChanged;
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(_abilities.Value, orderImpactsHashcode: false);
    }

    public void Initialize(Card card, Game game)
    {
      Game = game;

      _abilities.Initialize(Game, card);
      _abilities.Changed += OnAbilitiesChanged;
      _cardBase.Changed += OnCardBaseChanged;

      foreach (var ability in _abilities.Value)
      {        
        ability.OnEnable();        
      }
    }

    public IEnumerable<ManaAbility> GetManaAbilities()
    {
      return _abilities.Value.OfType<ManaAbility>();
    }

    public void Activate(int abilityIndex, ActivationParameters activationParameters)
    {
      _abilities.Value[abilityIndex].Activate(activationParameters);
    }

    public List<ActivationPrerequisites> CanActivate(bool ignoreManaAbilities)
    {
      var result = new List<ActivationPrerequisites>();

      for (var index = 0; index < _abilities.Value.Count; index++)
      {
        var ability = _abilities.Value[index];

        if (ignoreManaAbilities && ability is ManaAbility)
        {
          continue;
        }

        var prerequisites = ability.GetPrerequisites();
        if (prerequisites.CanBePlayedAndPayed)
        {
          prerequisites.Index = index;
          result.Add(prerequisites);
        }
      }

      return result;
    }

    public ManaAmount GetManaCost(int index)
    {
      return _abilities.Value[index].GetManaCost();
    }

    public IEnumerable<ManaAmount> GetManaCost()
    {
      return _abilities.Value.Select(x => x.GetManaCost());
    }

    public void AddModifier(PropertyModifier<List<ActivatedAbility>> modifier)
    {
      _abilities.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<List<ActivatedAbility>> modifier)
    {
      _abilities.RemoveModifier(modifier);
    }

    private void OnCardBaseChanged()
    {
      _abilities.ChangeBaseValue(_cardBase.Value.ActivatedAbilities);
    }
  }
}