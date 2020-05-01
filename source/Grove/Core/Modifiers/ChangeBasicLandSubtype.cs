namespace Grove.Modifiers
{
  using System.Collections.Generic;

  public class ChangeBasicLandSubtype : Modifier, ICardModifier
  {
    private readonly string _landSubtype;
    private readonly bool _replace;
    private ActivatedAbilities _abilities;
    private ActivatedAbility _addedAbility;
    private TypeOfCard _typeOfCard;
    private CardTypeSetter _cardTypeModifier;
    private PropertyModifier<List<ActivatedAbility>> _modifier;

    private static Dictionary<string, string> BasicLandTypeToManaSymbol = new Dictionary<string, string>()
    {
      {"swamp", "{B}"},
      {"island", "{U}"},
      {"mountain", "{R}"},
      {"forest", "{G}"},
      {"plains", "{W}"},
    };

    private ChangeBasicLandSubtype() {}

    public ChangeBasicLandSubtype(string landSubtype, bool replace)
    {
      _landSubtype = landSubtype.ToLowerInvariant();
      _replace = replace;
    }

    public override void Apply(ActivatedAbilities abilities)
    {
      _abilities = abilities;

      var ap = new ManaAbilityParameters
        {
          Text = string.Format("{{T}}: Add {0} to your mana pool.", 
          BasicLandTypeToManaSymbol[_landSubtype]),
        };

      ap.ManaAmount(Mana.GetBasicLandMana(_landSubtype));
      _addedAbility = new ManaAbility(ap);
      _addedAbility.Initialize(OwningCard, Game);

      if (_replace)
      {
        _modifier = new SetList<ActivatedAbility>(new List<ActivatedAbility> {_addedAbility});
      }
      else
      {
        _modifier = new AddToList<ActivatedAbility>(_addedAbility);
      }

      _modifier.Initialize(ChangeTracker);
      _abilities.AddModifier(_modifier);
    }

    public override void Apply(TypeOfCard typeOfCard)
    {
      _typeOfCard = typeOfCard;

      var type = _typeOfCard.Value.Change(subTypes: _landSubtype);
      _cardTypeModifier = new CardTypeSetter(type);
      _cardTypeModifier.Initialize(ChangeTracker);

      _typeOfCard.AddModifier(_cardTypeModifier);
    }

    protected override void Unapply()
    {
      _typeOfCard.RemoveModifier(_cardTypeModifier);
      _abilities.RemoveModifier(_modifier);
    }
  }
}