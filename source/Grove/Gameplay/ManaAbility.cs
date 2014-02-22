namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Gameplay.Effects;
  using Grove.Infrastructure;

  public class ManaAbility : ActivatedAbility, IManaSource, ICopyContributor
  {
    private readonly int _costRestriction;
    private readonly ManaOutput _manaOutput;
    private readonly int _priority;
    private readonly bool _tapRestriction;
    private readonly bool _sacRestriction;
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();
    private readonly ManaUsage _usageRestriction;

    private ManaAbility() {}

    public ManaAbility(ManaAbilityParameters p) : base(p)
    {
      _manaOutput = p.ManaOutput;
      _priority = p.Priority;
      _costRestriction = p.CostRestriction;
      _tapRestriction = p.TapRestriction;
      _sacRestriction = p.SacRestriction;
      _usageRestriction = p.UsageRestriction;
    }

    public void AfterMemberCopy(object original)
    {
      SubscribeToEvents();
    }

    bool IManaSource.CanActivate()
    {
      return IsEnabled && CanPay().CanPay().Value;
    }

    void IManaSource.PayActivationCost()
    {      
      Pay();     
    }

    public IEnumerable<ManaUnit> GetUnits()
    {
      return _units;
    }

    protected override Effect CreateEffect(ActivationParameters p)
    {
      var effectParameters = new EffectParameters
        {
          Source = this,
          X = p.X
        };

      return new AddManaToPool(_manaOutput.GetAmount(), _usageRestriction)
        .Initialize(effectParameters, Game);
    }

    private void SubscribeToEvents()
    {
      _manaOutput.Increased = OnOutputIncreased;
      _manaOutput.Decreased = OnOutputDecreased;

      OwningCard.JoinedBattlefield += delegate { ActivateSource(); };
      OwningCard.LeftBattlefield += delegate { DeactivateSource(); };
    }

    public override void OnAbilityRemoved()
    {
      DeactivateSource();
    }

    public override void OnAbilityAdded()
    {
      if (OwningCard != null && OwningCard.Zone == Zone.Battlefield)
        ActivateSource();
    }

    private void OnOutputIncreased(IManaAmount amount)
    {
      AddUnits(amount);
    }

    private void OnOutputDecreased(IManaAmount amount)
    {
      RemoveUnits(amount);
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      _manaOutput.Initialize(this, game);
      _units.Initialize(ChangeTracker);

      SubscribeToEvents();
    }

    private void ActivateSource()
    {
      var amount = _manaOutput.GetAmount();
      AddUnits(amount);

      Subscribe(_manaOutput);
    }

    private void DeactivateSource()
    {
      foreach (var unit in _units)
      {
        OwningCard.Controller.RemoveManaSource(unit);
      }

      _units.Clear();
      Unsubscribe(_manaOutput);
    }

    public void AddAditionalAmountAbilityWillProduce(IManaAmount amount)
    {
      AddUnits(amount);
      _manaOutput.AddAditional(amount);
    }

    private void AddUnits(IManaAmount amount)
    {
      foreach (var singleColor in amount)
      {
        for (var i = 0; i < singleColor.Count; i++)
        {
          var unit = CreateManaUnit(singleColor.Color);

          _units.Add(unit);
          OwningCard.Controller.AddManaSource(unit);
        }
      }
    }

    private void RemoveUnits(IManaAmount amount)
    {
      if (_units.Count == 0)
        return;

      foreach (var singleColor in amount)
      {
        for (var i = 0; i < singleColor.Count; i++)
        {
          var unit = _units.FirstOrDefault(x => x.Color == singleColor.Color);
          if (unit == null)
            break;

          _units.Remove(unit);
          OwningCard.Controller.RemoveManaSource(unit);
        }
      }
    }

    public void RemoveAdditionalAmountAbilityWillProduce(IManaAmount amount)
    {
      RemoveUnits(amount);
      _manaOutput.RemoveAdditional(amount);
    }


    private ManaUnit CreateManaUnit(ManaColor color)
    {
      return new ManaUnit(
        color,
        _priority,
        this,
        _tapRestriction ? OwningCard : null,
        _sacRestriction ? OwningCard : null,
        _costRestriction,
        _usageRestriction);
    }
  }
}