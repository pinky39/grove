namespace Grove
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  public class StaticAbilities : GameObject, IHashable, ICopyContributor, IAcceptsCardModifier
  {
    private readonly Characteristic<List<StaticAbility>> _abilities;
    private readonly CardBase _cardBase;

    private StaticAbilities() {}

    public StaticAbilities(CardBase cardBase)
    {
      _cardBase = cardBase;      

      _abilities = new Characteristic<List<StaticAbility>>(cardBase.Value.StaticAbilities);
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
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
      _cardBase.Changed += OnCardBaseChanged;
    }

    public void AddModifier(PropertyModifier<List<StaticAbility>> modifier)
    {
      _abilities.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<List<StaticAbility>> modifier)
    {
      _abilities.RemoveModifier(modifier);
    }

    private void OnCardBaseChanged()
    {
      _abilities.ChangeBaseValue(_cardBase.Value.StaticAbilities);
    }
  }
}