namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Effects;
  using Infrastructure;
  using Triggers;

  public class TriggeredAbility : Ability, ICopyContributor, IHashable
  {
    private readonly Parameters _p;    
    private TriggeredAbility() {}

    public TriggeredAbility(Parameters p) : base(p)
    {
      _p = p;
    }

    public IEnumerable<Trigger> Triggers
    {
      get { return _p.Triggers; }
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var trigger in _p.Triggers)
      {
        RegisterTriggerListener(trigger);
      }

      SubscribeToEvents();
    }

    public void OnEnable()
    {
      if (!_p.TriggerOnlyIfOwningCardIsInPlay || OwningCard.Zone == Zone.Battlefield)
      {
        ActivateTriggers();
      }

      SubscribeToEvents();
    }
        
    public void OnDisable()
    {
      UnsubscribeFromEvents();
      DeactivateTriggers();      
    }

    private void OnOwningCardJoinedBattlefield()
    {
      if (_p.TriggerOnlyIfOwningCardIsInPlay)
      {
        ActivateTriggers();
      }
    }

    private void OnOwningCardLeftBattlefield()
    {
      if (_p.TriggerOnlyIfOwningCardIsInPlay)
      {
        DeactivateTriggers();
      }
    }

    private void SubscribeToEvents()
    {
      OwningCard.JoinedBattlefield += OnOwningCardJoinedBattlefield;
      OwningCard.LeftBattlefield += OnOwningCardLeftBattlefield;
    }

    private void UnsubscribeFromEvents()
    {
      OwningCard.JoinedBattlefield -= OnOwningCardJoinedBattlefield;
      OwningCard.LeftBattlefield -= OnOwningCardLeftBattlefield;
    }

    public int CalculateHash(HashCalculator calc)
    {      
      return calc.Calculate(_p.Triggers, 
        orderImpactsHashcode: false);      
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      foreach (var trigger in _p.Triggers)
      {
        trigger.Initialize(this, Game);
        RegisterTriggerListener(trigger);
      }     
    }

    private void ActivateTriggers()
    {
      foreach (var trigger in _p.Triggers)
      {
        trigger.Activate();
      }
    }

    private void DeactivateTriggers()
    {
      foreach (var trigger in _p.Triggers)
      {
        trigger.Deactivate();
      }
    }

    protected virtual void Execute(object triggerMessage)
    {      
      var effectParameters = new EffectParameters
      {
        Source = this,
        TriggerMessage = triggerMessage
      };

      var effect = _p.Effect().Initialize(effectParameters, Game);
      
      if (_p.TargetSelector.Count > 0)
      {
        Enqueue(new SetTriggeredAbilityTarget(
          OwningCard.Controller,
          p =>
            {
              p.Effect = effect;              
              p.TargetSelector = _p.TargetSelector;              
              p.MachineRules = _p.Rules;
              p.UsesStack = _p.UsesStack;
              p.DistributeAmount = _p.DistributeAmount;
            }));

        return;
      }            
      Resolve(effect);
    }

    private void Resolve(Effect e)
    {
      if (_p.UsesStack == false)
      {
        e.BeginResolve();
        e.FinishResolve();
        return;
      }

      Stack.QueueTriggered(e);
    }

    private void RegisterTriggerListener(Trigger trigger)
    {
      trigger.Triggered += (o, e) => Execute(e.TriggerMessage);
    }

    public class Parameters : AbilityParameters
    {
      private readonly List<Trigger> _triggers = new List<Trigger>();

      public bool TriggerOnlyIfOwningCardIsInPlay;
      public List<Trigger> Triggers { get { return _triggers; } }

      public void Trigger(Trigger trigger)
      {
        _triggers.Add(trigger);
      }
    }
  }
}