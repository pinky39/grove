namespace Grove.Core
{
  using System;
  using Effects;
  using Mana;
  using Zones;

  //public class ManaAbility : ActivatedAbility, IManaSource
  //{
  //  private Func<ManaAbility, Game, IManaAmount> _getManaAmount;
  //  private readonly ManaAmountCharacteristic _manaAmountCharacteristic = new ManaAmountCharacteristic(ManaAmount.Zero);

  //  public ManaAbility(ManaAbilityParameters p) : base(p)
  //  {
  //    _getManaAmount = p.GetManaAmount;
  //    Priority = p.Priority;

  //    EffectFactory = () => new AddManaToPool(
  //      amount: new DynParam<IManaAmount>((e, g) =>
  //        {
  //          var source = (ManaAbility) e.Source;
  //          return source.GetManaAmount();
  //        }));
  //  }

  //  protected ManaAbility() {}

  //  public int Priority { get; private set; }
  //  object IManaSource.Resource { get { return OwningCard; } }

  //  public void Consume(IManaAmount amount, ManaUsage usage)
  //  {
  //    var produced = GetManaAmount();
      
  //    Pay();
  //    OwningCard.IncreaseUsageScore();
      
  //    if (amount.Converted == produced.Converted)
  //      return;

  //    // if effect produces more mana than needed 
  //    // add overflow mana to manapool.          
  //    var manaBag = new ManaBag(produced);
  //    manaBag.Consume(amount);
  //    OwningCard.Controller.AddManaToManaPool(manaBag.GetAmount());
  //  }

  //  public IManaAmount GetAvailableMana(ManaUsage usage)
  //  {
  //    ActivationPrerequisites prerequisites;
  //    return CanActivate(out prerequisites) == false ? ManaAmount.Zero : GetManaAmount();
  //  }

  //  private IManaAmount GetManaAmount()
  //  {
  //    return new AggregateManaAmount(_manaAmountCharacteristic.Value, _getManaAmount(this, Game));
  //  }

  //  public override bool CanActivate(out ActivationPrerequisites prerequisites)
  //  {      
  //    prerequisites = null;

  //    if (IsEnabled && OwningCard.Zone == Zone.Battlefield && CanPay().CanPay)
  //    {
  //      prerequisites =  new ActivationPrerequisites
  //      {
  //        Card = OwningCard,
  //        Description = Text,
  //        Selector = TargetSelector,
  //        DistributeAmount = DistributeAmount,          
  //        Rules = Rules,
  //      };
  //      return true;
  //    }

  //    return false;
  //  }

  //  public void AddManaModifier(PropertyModifier<IManaAmount> modifier)
  //  {
  //    _manaAmountCharacteristic.AddModifier(modifier);
  //  }

  //  public void RemoveManaModifier(PropertyModifier<IManaAmount> modifier)
  //  {
  //    _manaAmountCharacteristic.RemoveModifier(modifier);
  //  }

  //  public override void Initialize(Card owningCard, Game game)
  //  {
  //    base.Initialize(owningCard, game);

  //    _manaAmountCharacteristic.Initialize(game, null);
  //  }

  //  public void SetManaAmount(IManaAmount manaAmount)
  //  {
  //    _getManaAmount = delegate { return manaAmount; };
  //  }
  //}
}