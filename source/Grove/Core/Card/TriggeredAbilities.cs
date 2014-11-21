namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  public class TriggeredAbilities : GameObject, IAcceptsCardModifier, IHashable, ICopyContributor
  {
    private readonly Characteristic<List<TriggeredAbility>> _abilities;
    private readonly CardBase _cardBase;

    private TriggeredAbilities() {}

    public TriggeredAbilities(CardBase cardBase)
    {
      _cardBase = cardBase;      

      _abilities = new Characteristic<List<TriggeredAbility>>(cardBase.Value.TriggeredAbilities);      
    }

    private void OnAbilitiesChanged(CharacteristicChangedParams<List<TriggeredAbility>> p)
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
      return calc.Calculate(_abilities.Value,
        orderImpactsHashcode: false);
    }


    public void Initialize(Card card, Game game)
    {
      Game = game;

      _abilities.Initialize(game, card);
      _abilities.Changed += OnAbilitiesChanged;
      _cardBase.Changed += OnCardBaseChanged;
      
      foreach (var ability in _abilities.Value)
      {        
        ability.OnEnable();
      }
    }

    public void AddModifier(PropertyModifier<List<TriggeredAbility>> modifier)
    {
      _abilities.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<List<TriggeredAbility>> modifier)
    {
      _abilities.RemoveModifier(modifier);
    }

    private void OnCardBaseChanged()
    {
      _abilities.ChangeBaseValue(_cardBase.Value.TriggeredAbilities);
    }
  }
}