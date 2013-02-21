namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Triggers;
  using Zones;

  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly bool _triggerOnlyIfOwningCardIsInPlay;
    private readonly List<Trigger> _triggers;

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
    }

    public void Dispose()
    {
      foreach (var trigger in _triggers)
      {
        trigger.Dispose();
      }
    }

    public override int CalculateHash(HashCalculator calc)
    {
      var hashcodes = _triggers.Select(calc.Calculate).ToList();

      return HashCalculator.Combine(
        base.CalculateHash(calc),
        HashCalculator.Combine(hashcodes)
        );
    }

    public override void Initialize(Card owner, Game game)
    {
      base.Initialize(owner, game);
      

      foreach (var trigger in _triggers)
      {
        trigger.Initialize(this, game);
        RegisterTriggerListener(trigger);
      }
    }  

    protected virtual void Execute(object triggerMessage)
    {
      if (!IsEnabled)
        return;

      if (_triggerOnlyIfOwningCardIsInPlay && OwningCard.Zone != Zone.Battlefield)
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