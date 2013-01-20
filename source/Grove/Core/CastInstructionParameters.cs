namespace Grove.Core
{
  using Ai;
  using Casting;
  using Costs;
  using Effects;
  using Mana;
  using Targeting;

  public class CastInstructionParameters
  {      
    public CardType CardType;
    public Cost Cost;
    public string Description;
    public bool DistributeDamage;
    public EffectFactory Effect;
    public MachinePlayRule[] Rules;
    public IManaAmount ManaCost;
    public string Name;
    public CastingRule Rule;
    public TargetSelector TargetSelector;

    ////private const string DefaultDescriptionFormat = "Cast {0}.";
    ////private const string KickerDescriptionFormat = "Cast {0} with kicker.";      
    
    //public CastInstructionParameters()
    //{                  
    //  Description = DefaultDescription;
    //  Effect = new CardBuilder().Effect<PutIntoPlay>();                              
    //}

    //public string DefaultDescription { get { return string.Format(DefaultDescriptionFormat, _cardName); } }
    //public string KickerDescription { get { return string.Format(KickerDescriptionFormat, _cardName); } }

    //public ICostFactory Cost
    //{
    //  get
    //  {
    //    if (_cost != null)
    //      return _cost;

    //    if (_manaCost == null)
    //    {
    //      return _cost = _builder.Cost<NoCost>();
    //    }

    //    return _cost = _builder.Cost<PayMana>(c =>
    //      {
    //        c.Amount = _manaCost;
    //        c.XCalculator = XCalculator;
    //      });
    //  }
    //  set { _cost = value; }
    //}    

    //private CastingRule CastingRuleFromCardType(CardType cardType)
    //{
    //  if (cardType.Instant)
    //    return new Instant();
    //  if (cardType.Sorcery)
    //    return new Sorcery();
    //  if (cardType.Land)
    //    return new Land();
    //  if (cardType.Aura)
    //    return new Aura();

    //  return new Permanent();
    //}

    //private TimingDelegate TimingFromCardType(CardType cardType)
    //{
    //  if (cardType.Creature)
    //    return Timings.Creatures();

    //  if (cardType.Land)
    //    return Timings.Lands();

    //  return Timings.NoRestrictions();
    //}
  }
}