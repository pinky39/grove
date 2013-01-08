namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Cards;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Mana;
  using Targeting;

  public class CardParameters
  {
    private static readonly Random Rnd = new Random();
    public readonly List<IActivatedAbilityFactory> ActivatedAbilities = new List<IActivatedAbilityFactory>();
    public readonly List<IContinuousEffectFactory> ContinuousEffects = new List<IContinuousEffectFactory>();
    public readonly List<Static> StaticAbilities = new List<Static>();
    public readonly List<ITriggeredAbilityFactory> TriggeredAbilities = new List<ITriggeredAbilityFactory>();
    private readonly List<CastInstructionParameters> _castInstructions = new List<CastInstructionParameters>();    
    public ManaColors Colors;
    public IDamagePreventionFactory[] DamagePrevention = new IDamagePreventionFactory[] {};    
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool Isleveler;
    public IManaAmount ManaCost;    
    public bool MayChooseNotToUntap;
    public string Name;
    public int? OverrideScore;    
    public int? Power;
    public string[] ProtectionsFromCardTypes;
    public ManaColors ProtectionsFromColors = ManaColors.None;
    public CardText Text = string.Empty;
    public int? Toughness;
    public CardType Type;

    public IEnumerable<CastInstructionParameters> CastInstructions
    {
      get
      {        
        if (_castInstructions.Count == 0)
        {
          yield return new CastInstructionParameters(Name, ManaCost, Type);
          yield break;
        }

        foreach (var castInstructionParameterse in _castInstructions)
        {
          yield return castInstructionParameterse;
        }
      }
    }

    public void AddCastInstruction(CastInstructionParameters parameters)
    {
      _castInstructions.Add(parameters);
    }
            
    public string Illustration
    {
      get
      {
        const int basicLandVersions = 4;

        if (Type.BasicLand)
        {
          return Name + Rnd.Next(1, basicLandVersions + 1);
        }

        return Name;
      }
    }
  }
}