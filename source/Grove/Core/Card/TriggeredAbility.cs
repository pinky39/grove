namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Effects;
  using Infrastructure;
  using Triggers;

  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly Parameters _p;
    private TriggeredAbility() {}

    public TriggeredAbility(Parameters p) : base(p)
    {
      _p = p;
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var trigger in _p.Triggers)
      {
        RegisterTriggerListener(trigger);
      }

      SubscribeToEvents();
    }

    public void Dispose()
    {
      DeactivateTriggers();
      UnsubscribeFromEvents();
    }

    private void OnOwningCardJoinedBattlefield(object sender, EventArgs eventArgs)
    {
      if (_p.TriggerOnlyIfOwningCardIsInPlay)
        ActivateTriggers();
    }

    private void OnOwningCardLeftBattlefield(object sender, EventArgs eventArgs)
    {
      if (_p.TriggerOnlyIfOwningCardIsInPlay)
        DeactivateTriggers();
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

    public override int CalculateHash(HashCalculator calc)
    {
      var hashcodes = _p.Triggers.Select(calc.Calculate).ToList();

      return HashCalculator.Combine(
        base.CalculateHash(calc),
        HashCalculator.Combine(hashcodes));
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      foreach (var trigger in _p.Triggers)
      {
        trigger.Initialize(this, Game);
        RegisterTriggerListener(trigger);
      }

      if (!_p.TriggerOnlyIfOwningCardIsInPlay || owningCard.Zone == Zone.Battlefield)
        ActivateTriggers();

      SubscribeToEvents();
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
      if (!IsEnabled)
        return;

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
            }));

        return;
      }
      
      
      Resolve(effect, skipStack: false);
    }

    private void RegisterTriggerListener(Trigger trigger)
    {
      trigger.Triggered += (o, e) => Execute(e.TriggerMessage);
    }

    public new class Parameters : Ability.Parameters
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