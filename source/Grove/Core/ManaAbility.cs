namespace Grove.Core
{
  using System;
  using Effects;
  using Mana;
  using Zones;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private Func<ManaAbility, Game, IManaAmount> _manaAmount;
    private ManaAmountCharacteristic _manaAmountCharacteristic;

    public ManaAbility(ManaAbilityParameters p) : base(p)
    {
      _manaAmount = p.ManaAmount;
      
      EffectFactory = (pr, _) =>
        {
          var effect = new AddManaToPool();
          var source = (ManaAbility) pr.Source;
          effect.Amount = source.GetManaAmount();
          return effect;
        };      
    }

    public int Priority { get; set; }
    object IManaSource.Resource { get { return OwningCard; } }

    public void Consume(IManaAmount amount, ManaUsage usage)
    {
      Pay();
      OwningCard.IncreaseUsageScore();

      if (amount.Converted == GetManaAmount().Converted)
        return;

      // if effect produces more mana than needed 
      // add overflow mana to manapool.          
      var manaBag = new ManaBag(amount);
      manaBag.Consume(amount);
      OwningCard.Controller.AddManaToManaPool(manaBag.GetAmount());
    }

    public IManaAmount GetAvailableMana(ManaUsage usage)
    {
      ActivationPrerequisites prerequisites;
      return CanActivate(out prerequisites) == false ? ManaAmount.Zero : GetManaAmount();
    }

    private IManaAmount GetManaAmount()
    {
      return new AggregateManaAmount(_manaAmountCharacteristic.Value, _manaAmount(this, Game));
    }

    public override bool CanActivate(out ActivationPrerequisites prerequisites)
    {
      int? maxX = null;
      prerequisites = null;

      if (IsEnabled && OwningCard.Zone == Zone.Battlefield && CanPay(ref maxX))
      {
        prerequisites = new ActivationPrerequisites {Description = Text};
      }

      return false;
    }

    public void AddManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.AddModifier(modifier);
    }

    public void RemoveManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.RemoveModifier(modifier);
    }

    public override void Initialize(Card owner, Game game)
    {
      base.Initialize(owner, game);

      _manaAmountCharacteristic = new ManaAmountCharacteristic(
        ManaAmount.Zero,
        ChangeTracker, null);
    }

    public void SetManaAmount(IManaAmount manaAmount)
    {
      _manaAmount = delegate { return manaAmount; };
    }
  }
}