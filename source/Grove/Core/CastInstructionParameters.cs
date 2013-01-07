namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Ai;
  using Cards;
  using Cards.Casting;
  using Cards.Costs;
  using Cards.Effects;
  using Dsl;
  using Mana;
  using Targeting;

  public class CastInstructionParameters
  {
    public readonly string DefaultDescription = "Cast {0}.";
    public readonly string KickerDescription = "Cast {0} with kicker.";
    
    private readonly List<ICostFactory> _costs = new List<ICostFactory>();
    private readonly CardBuilder _builder = new CardBuilder();
    
    public CastInstructionParameters(string cardName, IManaAmount manaCost, CardType cardType)
    {      
      _costs.Add(_builder.Cost<PayMana>(c => c.Amount = manaCost));
      Rule = CastingRuleFromCardType(cardType);
      Description = string.Format(DefaultDescription, cardName);
      Effect = _builder.Effect<PutIntoPlay>();
      Timing = TimingFromCardType(cardType);
      Category = EffectCategories.Generic;
    }

    private ICastingRuleFactory CastingRuleFromCardType(CardType cardType)
    {
      if (cardType.Instant)      
        return _builder.Rule<Instant>();      
      if (cardType.Sorcery)
        return _builder.Rule<Sorcery>();
      if (cardType.Land)
        return _builder.Rule<Land>();
      if (cardType.Aura)
        return _builder.Rule<Aura>();

      return _builder.Rule<Permanent>();
    }

    private TimingDelegate TimingFromCardType(CardType cardType)
    {
      if (cardType.Creature)
        return Timings.Creatures();

      if (cardType.Land)
        return Timings.Lands();

      return Timings.NoRestrictions();
    }

    public ICostFactory AdditionalCost { set { _costs.Add(value); } }

    public ICostFactory Cost {get
    {
      return new Cost.Factory<AggregateCost> {Init = c => c.CostsFactories = _costs};
    }}
     

    public ICastingRuleFactory Rule;
    public IList<ITargetValidatorFactory> CostTargets = new List<ITargetValidatorFactory>();
    public string Description;
    public bool DistributeDamage;
    public IEffectFactory Effect;
    public IList<ITargetValidatorFactory> EffectTargets = new List<ITargetValidatorFactory>();
    public TargetSelectorAiDelegate TargetSelectorAi;
    public TimingDelegate Timing;
    public CalculateX XCalculator;
    public EffectCategories Category;
  }
}