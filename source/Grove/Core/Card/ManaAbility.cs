namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Effects;
  using Grove.Infrastructure;

  public class ManaAbility : ActivatedAbility, IManaSource, ICopyContributor
  {
    private readonly Parameters _p;
    private readonly TrackableList<ManaUnit> _units = new TrackableList<ManaUnit>();

    private ManaAbility() { }

    public ManaAbility(Parameters p) : base(p) { _p = p; }

    public void AfterMemberCopy(object original) { SubscribeToEvents(); }

    bool IManaSource.CanActivate() { return IsEnabled && CanPay().CanPay().Value; }

    void IManaSource.PayActivationCost() { Pay(); }

    public IEnumerable<ManaUnit> GetUnits() { return _units; }

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

    public override void OnAbilityRemoved() { DeactivateSource(); }

    public override void OnAbilityAdded()
    {
      if (OwningCard != null && OwningCard.Zone == Zone.Battlefield)
        ActivateSource();
    }

    private void OnOutputIncreased(IManaAmount amount) { AddUnits(amount); }

    private void OnOutputDecreased(IManaAmount amount) { RemoveUnits(amount); }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      _p.ManaOutput.Initialize(this, game);
      _units.Initialize(ChangeTracker);

      SubscribeToEvents();
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
        OwningCard.Controller.RemoveManaSource(unit);
      }

      _units.Clear();
      Unsubscribe(_p.ManaOutput);
    }

    public void AddAditionalAmountAbilityWillProduce(IManaAmount amount)
    {
      AddUnits(amount);
      _p.ManaOutput.AddAditional(amount);
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
      _p.ManaOutput.RemoveAdditional(amount);
    }


    private ManaUnit CreateManaUnit(ManaColor color)
    {
      return new ManaUnit(
        color,
        _p.Priority,
        this,
        _p.TapRestriction ? OwningCard : null,
        _p.SacRestriction ? OwningCard : null,
        _p.CostRestriction,
        _p.UsageRestriction);
    }

    public new class Parameters : ActivatedAbility.Parameters
    {
      public int CostRestriction;
      public int Priority = ManaSourcePriorities.Land;
      public bool SacRestriction;
      public bool TapRestriction;
      public ManaUsage UsageRestriction = ManaUsage.Any;

      public Parameters() { UsesStack = false; }

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