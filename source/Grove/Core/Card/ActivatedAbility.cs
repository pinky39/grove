namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using Costs;
  using Effects;
  using Events;
  using Infrastructure;

  public class ActivatedAbility : Ability
  {
    private readonly Trackable<int> _lastActivation = new Trackable<int>();
    private readonly ActivatedAbilityParameters _p;

    protected ActivatedAbility() {}

    public ActivatedAbility(ActivatedAbilityParameters p)
      : base(p)
    {
      _p = p;
    }

    public bool IsEquip
    {
      get { return _p.IsEquip; }
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
        var effect = CreateEffect();

        var effectParameters = new EffectParameters
        {
          Source = this,
          Targets = p.Targets,
          X = p.X
        };

        effect.Initialize(effectParameters, Game);                
        effects.Add(effect);
      }

      Pay(p);

      foreach (var effect in effects)
      {
        if (p.SkipStack)
        {
          Resolve(effect, true);
          continue;
        }

        Publish(new AbilityActivatedEvent(this, p.Targets));
        
        Resolve(effect, false);
        Publish(new ActivatedAbilityPutOnStackEvent(this, p.Targets));
      }
    }

    public virtual void OnAbilityRemoved() {}
    public virtual void OnAbilityAdded() {}

    public IManaAmount GetManaCost()
    {
      return _p.Cost.GetManaCost();
    }

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

    protected virtual Effect CreateEffect()
    {
      return _p.Effect();            
    }

    protected void Pay(ActivationParameters p = null)
    {
      p = p ?? new ActivationParameters();

      if (p.PayCost)
      {
        _p.Cost.Pay(p.Targets, p.X, p.Repeat);
      }
    }

    protected CanPayResult CanPay()
    {
      return _p.Cost.CanPay();
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != _p.ActivationZone)
        return false;

      if (_p.ActivateOnlyOnceEachTurn && _lastActivation.Value == Turn.TurnCount)
      {
        return false;
      }

      if (!_p.Condition(this))
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
  }
}