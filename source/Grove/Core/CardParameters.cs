namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Ai;
  using Cards;
  using Cards.Casting;
  using Cards.Costs;
  using Cards.Effects;
  using Cards.Modifiers;
  using Cards.Preventions;
  using Mana;
  using Targeting;
  using Zones;

  public class CardParameters
  {
    private static readonly Random Rnd = new Random();

    public bool DistributeDamage;
    public IEffectFactory EffectFactory = new Effect.Factory<PutIntoPlay>();
    public CardText FlavorText = string.Empty;
    public IManaAmount KickerCost;
    public IEffectFactory KickerEffectFactory;
    public IManaAmount ManaCost;
    public bool MayChooseNotToUntap;
    public string Name;
    public int? OverrideScore;
    public CardText Text = string.Empty;
    public CardType Type;
    public CalculateX XCalculator;
    public int? Power;
    public int? Toughness;
    public bool Isleveler;
    public ManaColors Colors;
    public string[] ProtectionsFromCardTypes;
    public ManaColors ProtectionsFromColors = ManaColors.None;
    public IDamagePreventionFactory[] DamagePreventionFactories = new IDamagePreventionFactory[] {};
    public EffectCategories EffectCategories;
    public TimingDelegate Timing;
    public ITargetValidatorFactory[] EffectValidatorFactories = new ITargetValidatorFactory[] {};
    public TargetSelectorAiDelegate AiTargetSelector;
    public ITargetValidatorFactory[] CostValidatorFactories = new ITargetValidatorFactory[] {};
    public ITargetValidatorFactory[] KickerEffectValidatorFactories = new ITargetValidatorFactory[] {};
    public TargetSelectorAiDelegate KickerAiTargetSelector;
    public ICostFactory AdditionalCost;
    public readonly List<Static> StaticAbilities = new List<Static>();
    public readonly List<ITriggeredAbilityFactory> TriggeredAbilityFactories = new List<ITriggeredAbilityFactory>();
    public readonly List<IActivatedAbilityFactory> ActivatedAbilityFactories = new List<IActivatedAbilityFactory>();
    public readonly List<IContinuousEffectFactory> ContinuousEffectFactories = new List<IContinuousEffectFactory>();
    public Zone? ResolveZone;

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

    public CastingRule CastingRule(Stack stack, TurnInfo turn)
    {
      if (Type.Instant)
        return new Instant(stack);

      if (Type.Land)
        return new Land(stack, turn);

      return new Default(stack, turn);
    } 
  }
}