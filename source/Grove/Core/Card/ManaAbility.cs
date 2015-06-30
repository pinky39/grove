namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Infrastructure;

  public class ManaAbility : ActivatedAbility, IManaSource, ICopyContributor
  {
    private readonly ManaAbilityParameters _p;
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();
    private readonly Trackable<bool> _isEnabled = new Trackable<bool>();
    private ManaCache _manaCache;
    private readonly ReentryGuard _guard = new ReentryGuard();

    private ManaAbility() {}    

    public ManaAbility(ManaAbilityParameters p) : base(p)
    {
      _p = p;
    }

    public void AfterMemberCopy(object original)
    {
      SubscribeToEvents();
    }

    bool IManaSource.CanActivate()
    {
      if (_guard.IsSet)
        return false;
      
      using (_guard.Set())
      {
        return _isEnabled && CanPay().CanPay;  
      }      
    }

    void IManaSource.PayActivationCost()
    {
      Pay();

      if (_p.AdditionalEffects.Count > 0)
      {
        var effect = new CompoundEffect(
          _p.AdditionalEffects.Select(factory => factory()).ToArray());

        var effectParameters = new EffectParameters
        {
          Source = this,
        };

        effect.Initialize(effectParameters, Game);

        Resolve(effect, skipStack: true);
      }
    }

    public IEnumerable<ManaUnit> GetUnits()
    {
      return _units;
    }

    public override void OnDisable()
    {
      DeactivateSource();
      base.OnDisable();
    }


    protected override Effect CreateEffect()
    {
      var addManaToPool = new AddManaToPool(_p.ManaOutput.GetAmount(), _p.UsageRestriction);

      if (_p.AdditionalEffects.Count == 0)
        return addManaToPool;

      var effects = addManaToPool
        .Concat(_p.AdditionalEffects.Select(x => x()))
        .ToArray();
            
      return new CompoundEffect(effects);            
    }

    public override void OnEnable()
    {
      if (OwningCard != null && OwningCard.Zone == Zone.Battlefield)
      {
        ActivateSource();
      }
      base.OnEnable();
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      _p.ManaOutput.Initialize(this, game);
      _units.Initialize(ChangeTracker);
      _manaCache = owningCard.Controller.ManaCache;
      _isEnabled.Initialize(ChangeTracker);

      SubscribeToEvents();
    }

    public void AddAditionalAmountAbilityWillProduce(ManaAmount amount)
    {
      AddUnits(amount);
      _p.ManaOutput.AddAditional(amount);
    }

    public void RemoveAdditionalAmountAbilityWillProduce(ManaAmount amount)
    {
      RemoveUnits(amount);
      _p.ManaOutput.RemoveAdditional(amount);
    }


    private void SubscribeToEvents()
    {
      _p.ManaOutput.Increased = OnOutputIncreased;
      _p.ManaOutput.Decreased = OnOutputDecreased;

      OwningCard.JoinedBattlefield += ActivateSource;
      OwningCard.LeftBattlefield += DeactivateSource;
    }

    private void OnOutputIncreased(ManaAmount amount)
    {
      AddUnits(amount);
    }

    private void OnOutputDecreased(ManaAmount amount)
    {
      RemoveUnits(amount);
    }

    private void ActivateSource()
    {
      var amount = _p.ManaOutput.GetAmount();
      AddUnits(amount);

      Subscribe(_p.ManaOutput);
      _isEnabled.Value = true;
    }

    private void DeactivateSource()
    {
      foreach (var unit in _units)
      {
        _manaCache.Remove(unit);
      }

      _units.Clear();
      Unsubscribe(_p.ManaOutput);
      _isEnabled.Value = false;
    }

    private void AddUnits(ManaAmount amount)
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

    private void RemoveUnits(ManaAmount amount)
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
  }
}