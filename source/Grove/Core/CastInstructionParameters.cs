namespace Grove.Core
{
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

    private readonly CardBuilder _builder = new CardBuilder();
    
    public EffectCategories Category;

    public IList<ITargetValidatorFactory> CostTargets = new List<ITargetValidatorFactory>();
    public string Description;
    public bool DistributeDamage;
    public IEffectFactory Effect;
    public IList<ITargetValidatorFactory> EffectTargets = new List<ITargetValidatorFactory>();
    public ICastingRuleFactory Rule;
    public TargetSelectorAiDelegate TargetSelectorAi;
    public TimingDelegate Timing;
    public CalculateX XCalculator;

    public CastInstructionParameters(string cardName, IManaAmount manaCost, CardType cardType)
    {
      Cost = _builder.Cost<PayMana>(c => c.Amount = manaCost);
      Rule = CastingRuleFromCardType(cardType);
      Description = string.Format(DefaultDescription, cardName);
      Effect = _builder.Effect<PutIntoPlay>();
      Timing = TimingFromCardType(cardType);
      Category = EffectCategories.Generic;
    }

    public ICostFactory Cost { get; set;  }

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
  }
}