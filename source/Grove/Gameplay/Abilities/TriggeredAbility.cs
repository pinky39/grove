namespace Grove.Gameplay.Abilities
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Effects;
  using Infrastructure;
  using Triggers;
  using Zones;

  [Serializable]
  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly bool _triggerOnlyIfOwningCardIsInPlay;
    private readonly List<Trigger> _triggers;

    private TriggeredAbility() {}

    public TriggeredAbility(TriggeredAbilityParameters p) : base(p)
    {
      _triggers = p.Triggers;
      _triggerOnlyIfOwningCardIsInPlay = p.TriggerOnlyIfOwningCardIsInPlay;
    }

    void ICopyContributor.AfterMemberCopy(object original)
    {
      foreach (var trigger in _triggers)
      {
        RegisterTriggerListener(trigger);
      }

      SubscribeToEvents();
    }

    public void Dispose()
    {
      foreach (var trigger in _triggers)
      {
        trigger.Deactivate();
      }
    }

    private void SubscribeToEvents()
    {
      OwningCard.JoinedBattlefield += delegate
        {
          if (_triggerOnlyIfOwningCardIsInPlay)
            ActivateTriggers();
        };

      OwningCard.LeftBattlefield += delegate
        {
          if (_triggerOnlyIfOwningCardIsInPlay)
            DeactivateTriggers();
        };
    }

    public override int CalculateHash(HashCalculator calc)
    {
      var hashcodes = _triggers.Select(calc.Calculate).ToList();

      return HashCalculator.Combine(
        base.CalculateHash(calc),
        HashCalculator.Combine(hashcodes)
        );
    }

    public override void Initialize(Card owningCard, Game game)
    {
      base.Initialize(owningCard, game);

      foreach (var trigger in _triggers)
      {
        trigger.Initialize(this, Game);
        RegisterTriggerListener(trigger);
      }

      if (!_triggerOnlyIfOwningCardIsInPlay || owningCard.Zone == Zone.Battlefield)
        ActivateTriggers();

      SubscribeToEvents();
    }

    private void ActivateTriggers()
    {
      foreach (var trigger in _triggers)
      {
        trigger.Activate();
      }
    }

    private void DeactivateTriggers()
    {
      foreach (var trigger in _triggers)
      {
        trigger.Deactivate();
      }
    }

    protected virtual void Execute(object triggerMessage)
    {
      if (!IsEnabled)
        return;

      if (TargetSelector.Count > 0)
      {
        Enqueue<SetTriggeredAbilityTarget>(
          OwningCard.Controller,
          p =>
            {
              p.Source = this;
              p.EffectFactory = EffectFactory;
              p.TriggerMessage = triggerMessage;
              p.TargetSelector = TargetSelector;
              p.MachineRules = Rules;
            });

        return;
      }

      var effectParameters = new EffectParameters
        {
          Source = this,
          TriggerMessage = triggerMessage
        };


      var effect = EffectFactory().Initialize(effectParameters, Game);
      Resolve(effect, skipStack: false);
    }

    private void RegisterTriggerListener(Trigger trigger)
    {
      trigger.Triggered += (o, e) => Execute(e.TriggerMessage);
    }
  }
}