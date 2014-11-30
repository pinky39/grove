namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  public class SimpleAbilities : GameObject, ISimpleAbilities, IAcceptsCardModifier, IHashable, ICopyContributor
  {
    private readonly CardBase _cardBase;    
    private readonly Characteristic<List<Static>> _abilities;

    public SimpleAbilities(CardBase cardBase)
    {
      _cardBase = cardBase;      

      _abilities = new Characteristic<List<Static>>(cardBase.Value.SimpleAbilities);      
    }

    private void OnCardBaseChanged()
    {
      _abilities.ChangeBaseValue(_cardBase.Value.SimpleAbilities);
    }

    private SimpleAbilities() {}

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public int CalculateHash(HashCalculator calc)
    {
      return calc.Calculate(
        _abilities.Value, 
        orderImpactsHashcode: false);
    }

    public bool Convoke
    {
      get { return Has(Static.Convoke); }
    }

    public bool Deathtouch
    {
      get { return Has(Static.Deathtouch); }
    }

    public bool Defender
    {
      get { return Has(Static.Defender); }
    }

    public bool Delve
    {
      get { return Has(Static.Delve); }
    }

    public bool Fear
    {
      get { return Has(Static.Fear); }
    }

    public bool Flying
    {
      get { return Has(Static.Flying); }
    }

    public bool Haste
    {
      get { return Has(Static.Haste); }
    }

    public bool Hexproof
    {
      get { return Has(Static.Hexproof); }
    }

    public bool Indestructible
    {
      get { return Has(Static.Indestructible); }
    }

    public bool Lifelink
    {
      get { return Has(Static.Lifelink); }
    }

    public bool Shroud
    {
      get { return Has(Static.Shroud); }
    }

    public bool Trample
    {
      get { return Has(Static.Trample); }
    }

    public bool Unblockable
    {
      get { return Has(Static.Unblockable); }
    }

    public bool FirstStrike
    {
      get { return Has(Static.FirstStrike); }
    }

    public bool DoubleStrike
    {
      get { return Has(Static.DoubleStrike); }
    }

    public bool Reach
    {
      get { return Has(Static.Reach); }
    }

    public bool Vigilance
    {
      get { return Has(Static.Vigilance); }
    }

    public bool Swampwalk
    {
      get { return Has(Static.Swampwalk); }
    }

    public bool CannotAttack
    {
      get { return Has(Static.CannotAttack); }
    }

    public bool CannotBlock
    {
      get { return Has(Static.CannotBlock); }
    }

    public bool Islandwalk
    {
      get { return Has(Static.Islandwalk); }
    }

    public bool Mountainwalk
    {
      get { return Has(Static.Mountainwalk); }
    }

    public bool AssignsDamageAsThoughItWasntBlocked
    {
      get { return Has(Static.AssignsDamageAsThoughItWasntBlocked); }
    }

    public bool CanAttackOnlyIfDefenderHasIslands
    {
      get { return Has(Static.CanAttackOnlyIfDefenderHasIslands); }
    }

    public bool UnblockableIfDedenderHasArtifacts
    {
      get { return Has(Static.UnblockableIfDedenderHasArtifacts); }
    }

    public bool UnblockableIfDedenderHasEnchantments
    {
      get { return Has(Static.UnblockableIfDedenderHasEnchantments); }
    }

    public bool Flash
    {
      get { return Has(Static.Flash); }
    }

    public bool AttacksEachTurnIfAble
    {
      get { return Has(Static.AttacksEachTurnIfAble); }
    }

    public bool Forestwalk
    {
      get { return Has(Static.Forestwalk); }
    }

    public bool Lure
    {
      get { return Has(Static.Lure); }
    }

    public bool Echo
    {
      get { return Has(Static.Echo); }
    }

    public bool DoesNotUntap
    {
      get { return Has(Static.DoesNotUntap); }
    }

    public bool AnyEvadingAbility
    {
      get
      {
        return Fear || Flying || Trample || Unblockable || AssignsDamageAsThoughItWasntBlocked ||
          CanOnlyBeBlockedByCreaturesWithFlying || CanOnlyBeBlockedByWalls || Swampwalk || Mountainwalk || Islandwalk;
      }
    }

    public bool CanBlockOnlyCreaturesWithFlying
    {
        get { return Has(Static.CanBlockOnlyCreaturesWithFlying); }
    }

    public bool CanOnlyBeBlockedByCreaturesWithFlying
    {
        get { return Has(Static.CanOnlyBeBlockedByCreaturesWithFlying); }
    }

    public bool CanOnlyBeBlockedByWalls 
    {
      get { return Has(Static.CanOnlyBeBlockedByWalls); }
    }

    public bool CannotBeBlockedByWalls
    {
        get { return Has(Static.CannotBeBlockedByWalls); }
    }

    public bool Intimidate
    {
        get { return Has(Static.Intimidate); }
    }

    public bool Has(Static ability)
    {
      return _abilities.Value.Any(x => x == ability);
    }

    public void Initialize(IHashDependancy hashDependancy, Game game)
    {
      Game = game;

      _abilities.Initialize(game, hashDependancy);
      _cardBase.Changed += OnCardBaseChanged;
    }

    public void AddModifier(PropertyModifier<List<Static>> modifier)
    {
      _abilities.AddModifier(modifier);
    }

    public void RemoveModifier(PropertyModifier<List<Static>> modifier)
    {
      _abilities.RemoveModifier(modifier);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
    }
  }
}