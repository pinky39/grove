namespace Grove.Core.Cards
{
  using Dsl;
  using Effects;
  using Mana;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private ManaAmountCharacteristic _manaAmount;

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

      if (amount.Converted == _manaAmount.Value.Converted)
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
      return prerequisites.CanBeSatisfied ? _manaAmount.Value : ManaAmount.Zero;
    }

    public override SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      if (IsEnabled && OwningCard.Zone == ActivationZone && Cost.CanPay(ref maxX))
      {
        return new SpellPrerequisites
          {
            CanBeSatisfied = true,
            Description = Text,
            Timming = delegate { return true; },
            IsAbility = true
          };
      }

      return SpellPrerequisites.CannotBeSatisfied;
    }

    public void AddManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmount.AddModifier(modifier);
    }

    public void RemoveManaModifier(PropertyModifier<IManaAmount> modifier)
    {
      _manaAmount.RemoveModifier(modifier);
    }

    public void SetManaAmount(IManaAmount manaAmount)
    {
      var builder = new CardBuilder();

      Effect(builder.Effect<AddManaToPool>
        (
          e =>
            {
              var manaAbility = (ManaAbility) e.Source;
              e.Amount = manaAbility._manaAmount.Value;
            }));

      _manaAmount = new ManaAmountCharacteristic(
        manaAmount, 
        Game.ChangeTracker, null);
    }
  }
}