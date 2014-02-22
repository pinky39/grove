namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;
  using Grove.Infrastructure;
  using Grove.Gameplay.Messages;

  public class ActivatedAbility : Ability
  {
    private readonly bool _activateAsSorcery;
    private readonly bool _activateOnlyOnceEachTurn;
    private readonly Zone _activationZone;
    private readonly Cost _cost;
    private readonly Trackable<int> _lastActivation = new Trackable<int>();

    protected ActivatedAbility() {}

    public ActivatedAbility(ActivatedAbilityParameters parameters) : base(parameters)
    {
      _cost = parameters.Cost;
      _activationZone = parameters.ActivationZone;
      _activateAsSorcery = parameters.ActivateAsSorcery;
      _activateOnlyOnceEachTurn = parameters.ActivateOnlyOnceEachTurn;
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

    public virtual void OnAbilityRemoved() {}

    public virtual void OnAbilityAdded() {}

    public IManaAmount GetManaCost()
    {
      return _cost.GetManaCost();
    }

    public override int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        base.CalculateHash(calc),
        _activateAsSorcery.GetHashCode(),
        calc.Calculate(_cost));
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
            Selector = TargetSelector,
            DistributeAmount = DistributeAmount,
            MaxX = result.MaxX(),
            Rules = Rules,
            MaxRepetitions = result.MaxRepetitions(),
          };

        return true;
      }
      return false;
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);
      _cost.Initialize(owningCard, game, TargetSelector.Cost.FirstOrDefault());
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

      return EffectFactory().Initialize(effectParameters, Game);
    }

    protected void Pay(ActivationParameters p = null)
    {
      p = p ?? new ActivationParameters();

      if (p.PayCost)
      {
        _cost.Pay(p.Targets, p.X, p.Repeat);
      }
    }

    protected CanPayResult CanPay()
    {
      return _cost.CanPay();
    }

    private bool CanBeActivatedAtThisTime()
    {
      if (OwningCard.Zone != _activationZone)
        return false;

      if (_activateOnlyOnceEachTurn && _lastActivation.Value == Turn.TurnCount)
      {
        return false;
      }

      if (_activateAsSorcery)
      {
        return Turn.Step.IsMain() &&
          OwningCard.Controller.IsActive &&
            Stack.IsEmpty;
      }

      return true;
    }
  }
}