namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Costs;
  using Effects;
  using Infrastructure;

  public class ManaAbility : ActivatedAbility, IManaSource, ICopyContributor
  {
    private readonly Parameters _p;
    private ManaCache _manaCache;
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();

    private ManaAbility() {}

    public ManaAbility(Parameters p) : base(p)
    {
      _p = p;
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

    public override void OnAbilityRemoved()
    {
      DeactivateSource();
    }

    public override void OnAbilityAdded()
    {
      if (OwningCard != null && OwningCard.Zone == Zone.Battlefield)
      {
        ActivateSource();
      }
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      _p.ManaOutput.Initialize(this, game);
      _units.Initialize(ChangeTracker);
      _manaCache = owningCard.Controller.ManaCache;

      SubscribeToEvents();
    }

    public void AddAditionalAmountAbilityWillProduce(IManaAmount amount)
    {
      AddUnits(amount);
      _p.ManaOutput.AddAditional(amount);
    }

    public void RemoveAdditionalAmountAbilityWillProduce(IManaAmount amount)
    {
      RemoveUnits(amount);
      _p.ManaOutput.RemoveAdditional(amount);
    }

    protected override Effect CreateEffect(ActivationParameters p)
    {
      var effectParameters = new EffectParameters
        {
          Source = this,
          X = p.X
        };

      return new AddManaToPool(_p.ManaOutput.GetAmount(), _p.UsageRestriction)
        .Initialize(effectParameters, Game);
    }

    private void SubscribeToEvents()
    {
      _p.ManaOutput.Increased = OnOutputIncreased;
      _p.ManaOutput.Decreased = OnOutputDecreased;

      OwningCard.JoinedBattlefield += delegate { ActivateSource(); };
      OwningCard.LeftBattlefield += delegate { DeactivateSource(); };
    }

    private void OnOutputIncreased(IManaAmount amount)
    {
      AddUnits(amount);
    }

    private void OnOutputDecreased(IManaAmount amount)
    {
      RemoveUnits(amount);
    }

    private void ActivateSource()
    {
      var amount = _p.ManaOutput.GetAmount();
      AddUnits(amount);

      Subscribe(_p.ManaOutput);
    }

    private void DeactivateSource()
    {
      foreach (var unit in _units)
      {
        _manaCache.Remove(unit);        
      }

      _units.Clear();
      Unsubscribe(_p.ManaOutput);
    }

    private void AddUnits(IManaAmount amount)
    {
      foreach (var singleColor in amount)
      {
        for (var i = 0; i < singleColor.Count; i++)
        {
          var unit = CreateManaUnit(singleColor.Color);

          _units.Add(unit);
          _manaCache.Add(unit);          
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
          _manaCache.Remove(unit);          
        }
      }
    }


    private ManaUnit CreateManaUnit(ManaColor color)
    {
      return new ManaUnit(
        color,
        _p.Priority,
        this,        
        _p.UsageRestriction);
    }

    public new class Parameters : ActivatedAbility.Parameters
    {
      public int Priority = ManaSourcePriorities.Land;
      public ManaUsage UsageRestriction = ManaUsage.Any;

      public Parameters()
      {
        UsesStack = false;
        Cost = new TapOwner();
      }

      public List<int> Colors { get; private set; }
      public ManaOutput ManaOutput { get; private set; }

      public void ManaAmount(IManaAmount amount)
      {
        ManaOutput = new FixedManaOutput(amount);
        Colors = amount.Colors.ToList();
      }

      public void ManaAmount(ManaColor color, Func<Card, bool> filter,
        ControlledBy controlledBy = ControlledBy.SpellOwner)
      {
        ManaOutput = new PermanentCountManaOutput(color, filter, controlledBy);
        Colors = color.Indices;
      }
    }
  }
}