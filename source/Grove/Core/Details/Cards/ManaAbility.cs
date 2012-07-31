namespace Grove.Core.Details.Cards
{
  using Effects;
  using Mana;

  public class ManaAbility : ActivatedAbility, IManaSource
  {
    private IManaAmount _manaAmount;

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

      if (amount.Converted == _manaAmount.Converted)
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
      return prerequisites.CanBeSatisfied ? _manaAmount : ManaAmount.Zero;
    }

    public override SpellPrerequisites CanActivate()
    {
      int? maxX = null;
      if (IsEnabled && Cost.CanPay(ref maxX))
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

    public void SetManaAmount(IManaAmount manaAmount)
    {
      Effect(new Effect.Factory<AddManaToPool>
        {
          Game = Game,
          Init = p => p.Effect.Amount = manaAmount
        });

      _manaAmount = manaAmount;
    }
  }
}