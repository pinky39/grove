﻿namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Costs;
  using Effects;
  using Events;
  using Grove.Infrastructure;

  public class ActivatedAbility : Ability
  {
    private readonly Parameters _p;
    private readonly Trackable<int> _lastActivation = new Trackable<int>();

    protected ActivatedAbility() { }

    public ActivatedAbility(Parameters p)
      : base(p)
    {
      _p = p;
    }

    public void Activate(ActivationParameters p)
    {
      _lastActivation.Value = Turn.TurnCount;

      var effects = new List<Effect>();

      for (var i = 0; i < p.Repeat; i++)
      {
        // create effect first, since some cost e.g Sacrifice can
        // put owning card to graveyard which will alter some card
        // properties e.g counters, power, toughness ...             
        effects.Add(CreateEffect(p));
      }

      Pay(p);

      foreach (var effect in effects)
      {
        if (p.SkipStack)
        {
          Resolve(effect, true);
          continue;
        }

        Publish(new BeforeActivatedAbilityWasPutOnStack(this, p.Targets));
        Resolve(effect, false);
        Publish(new AfterActivatedAbiltyWasPutOnStack(this, p.Targets));
      }
    }

    public virtual void OnAbilityRemoved() { }
    public virtual void OnAbilityAdded() { }
    public IManaAmount GetManaCost() { return _p.Cost.GetManaCost(); }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        _p.ActivateAsSorcery.GetHashCode(),
        calc.Calculate(_p.Cost));
    }

    public virtual bool CanActivate(out ActivationPrerequisites activationPrerequisites)
    {
      activationPrerequisites = null;

      if (IsEnabled && CanBeActivatedAtThisTime())
      {
        var result = CanPay();

        activationPrerequisites = new ActivationPrerequisites
          {
            CanPay = result.CanPay(),
            Card = OwningCard,
            Description = Text,
            Selector = _p.TargetSelector,
            DistributeAmount = _p.DistributeAmount,
            MaxX = result.MaxX(),
            Rules = _p.Rules,
            MaxRepetitions = result.MaxRepetitions(),
          };

        return true;
      }
      return false;
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);
      _p.Cost.Initialize(owningCard, game, _p.TargetSelector.Cost.FirstOrDefault());
      _lastActivation.Initialize(ChangeTracker);
    }

    protected virtual Effect CreateEffect(ActivationParameters p)
    {
      var effectParameters = new EffectParameters
        {
          Source = this,
          Targets = p.Targets,
          X = p.X
        };

      return _p.Effect().Initialize(effectParameters, Game);
    }

    protected void Pay(ActivationParameters p = null)
    {
      p = p ?? new ActivationParameters();

      if (p.PayCost)
      {
        _p.Cost.Pay(p.Targets, p.X, p.Repeat);
      }
    }

    protected CanPayResult CanPay() { return _p.Cost.CanPay(); }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != _p.ActivationZone)
        return false;

      if (_p.ActivateOnlyOnceEachTurn && _lastActivation.Value == Turn.TurnCount)
      {
        return false;
      }

      if (_p.ActivateAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
          Stack.IsEmpty;
      }

      return true;
    }

    public new class Parameters : Ability.Parameters
    {
      public bool ActivateAsSorcery;
      public bool ActivateOnlyOnceEachTurn;
      public Zone ActivationZone = Zone.Battlefield;
      public Cost Cost;
    }
  }  
}