namespace Grove.Core.Cards
{
  using System;
  using Dsl;
  using Effects;
  using Mana;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private Func<ManaAbility, Game, IManaAmount> _manaAmount;
    private ManaAmountCharacteristic _manaAmountCharacteristic;

    public ManaAbility()
    {
      UsesStack = false;
    }

    public int Priority { get; set; }
    object IManaSource.Resource { get { return OwningCard; } }

    public void Consume(IManaAmount amount, ManaUsage usage)
    {
      Cost.Pay(target: null, x: null);
      OwningCard.IncreaseUsageScore();

      if (amount.Converted == GetManaAmount().Converted)
        return;

      // if effect produces more mana than needed 
      // add overflow mana to manapool.          
      var manaBag = new ManaBag(amount);
      manaBag.Consume(amount);
      Controller.AddManaToManaPool(manaBag.GetAmount());
    }

    public IManaAmount GetAvailableMana(ManaUsage usage)
    {
      var prerequisites = CanActivate();

      if (prerequisites.CanBeSatisfied == false)
        return ManaAmount.Zero;

      return GetManaAmount();
    }   

    private IManaAmount GetManaAmount()
    {
      return new AggregateManaAmount(_manaAmountCharacteristic.Value, _manaAmount(this, Game));
    }

    public override SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      if (IsEnabled && OwningCard.Zone == ActivationZone && Cost.CanPay(ref maxX))
      {
        return new SpellPrerequisites
          {
            CanBeSatisfied = true,
            Description = Text
          };
      }

      return SpellPrerequisites.CannotBeSatisfied;
    }

    public void AddManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.AddModifier(modifier);
    }

    public void RemoveManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmountCharacteristic.RemoveModifier(modifier);
    }

    public void SetManaAmount(Func<ManaAbility, Game, IManaAmount> getManaAmount)
    {
      _manaAmount = getManaAmount;
    }

    protected override void Initialize()
    {
      _manaAmountCharacteristic = new ManaAmountCharacteristic(
        ManaAmount.Zero,
        Game.ChangeTracker, null);

      CreateEffectFactory();
    }

    private void CreateEffectFactory()
    {
      var builder = new CardBuilder();
      Effect(builder.Effect<AddManaToPool>(e => { e.Amount = GetManaAmount(); }));
    }

    public void SetManaAmount(IManaAmount manaAmount)
    {
      _manaAmount = delegate { return manaAmount; };
    }
  }
}