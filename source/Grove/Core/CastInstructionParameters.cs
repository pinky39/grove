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
    private const string DefaultDescriptionFormat = "Cast {0}.";
    private const string KickerDescriptionFormat = "Cast {0} with kicker.";

    private readonly CardBuilder _builder = new CardBuilder();
    private readonly string _cardName;
    private readonly IManaAmount _manaCost;

    public EffectCategories Category;

    public IList<ITargetValidatorFactory> CostTargets = new List<ITargetValidatorFactory>();
    public string Description;
    public bool DistributeDamage;
    public IEffectFactory Effect;
    public IList<ITargetValidatorFactory> EffectTargets = new List<ITargetValidatorFactory>();
    public ICastingRuleFactory Rule;
    public TargetingAiDelegate TargetingAi;
    public TimingDelegate Timing;
    public CalculateX XCalculator;

    private ICostFactory _cost;

    public CastInstructionParameters(string cardName, IManaAmount manaCost, CardType cardType)
    {
      _cardName = cardName;
      _manaCost = manaCost;
      
      Rule = CastingRuleFromCardType(cardType);
      Description = DefaultDescription;
      Effect = _builder.Effect<PutIntoPlay>();
      Timing = TimingFromCardType(cardType);
      Category = EffectCategories.Generic;
    }

    public string DefaultDescription { get { return string.Format(DefaultDescriptionFormat, _cardName); } }
    public string KickerDescription { get { return string.Format(KickerDescriptionFormat, _cardName); } }

    public ICostFactory Cost
    {
      get
      {
        if (_cost != null)
          return _cost;

        if (_manaCost == null)
        {
          return _cost = _builder.Cost<NoCost>();
        }

        return _cost = _builder.Cost<PayMana>(c =>
          {
            c.Amount = _manaCost;
            c.XCalculator = XCalculator;
          });
      }
      set { _cost = value; }
    }

    public bool HasXInCost { get { return XCalculator != null; }}

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