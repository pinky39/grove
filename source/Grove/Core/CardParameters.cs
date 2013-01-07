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
    //public ICostFactory AdditionalCost;
    public ManaColors Colors;
    public IDamagePreventionFactory[] DamagePrevention = new IDamagePreventionFactory[] {};

    //public bool DistributeDamage;
    //public IEffectFactory EffectFactory = new Effect.Factory<PutIntoPlay>();
    public CardText FlavorText = string.Empty;
    public bool HasXInCost;
    public bool Isleveler;
    public IManaAmount ManaCost;
    //public IManaAmount KickerCost;
    //public IEffectFactory KickerEffectFactory;
    //public IManaAmount ManaCost;
    public bool MayChooseNotToUntap;
    public string Name;
    public int? OverrideScore;
    //public CalculateX XCalculator;
    public int? Power;
    public string[] ProtectionsFromCardTypes;
    public ManaColors ProtectionsFromColors = ManaColors.None;
    public CardText Text = string.Empty;
    public int? Toughness;
    public CardType Type;

    public IList<CastInstructionParameters> CastInstructions
    {
      get
      {
        if (_castInstructions.Count == 0)
          return new[] {new CastInstructionParameters(Name, ManaCost, Type)};

        return _castInstructions;
      }
    }

    //public EffectCategories EffectCategories;
    //public TimingDelegate Timing;
    //public ITargetValidatorFactory[] EffectValidatorFactories = new ITargetValidatorFactory[] {};
    //public TargetSelectorAiDelegate AiTargetSelector;
    //public ITargetValidatorFactory[] CostValidatorFactories = new ITargetValidatorFactory[] {};
    //public ITargetValidatorFactory[] KickerEffectValidatorFactories = new ITargetValidatorFactory[] {};
    //public TargetSelectorAiDelegate KickerAiTargetSelector;
    //public Zone? ResolveZone;

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