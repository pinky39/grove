namespace Grove.Core.Details.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Infrastructure;
  using Targeting;
  using Triggers;
  using Zones;

  [Copyable]
  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly List<Trigger> _triggers = new List<Trigger>();
    private Decisions _decisions;
    private Trackable<bool> _isEnabled;

    public TriggeredAbility()
    {
      UsesStack = true;
    }

    public bool TriggerOnlyIfOwningCardIsInPlay { get; set; }
    private bool IsEnabled { get { return _isEnabled.Value; } set { _isEnabled.Value = value; } }

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

    public void AddTrigger(ITriggerFactory triggerFactory)
    {
      var trigger = triggerFactory.CreateTrigger(this);
      _triggers.Add(trigger);
      RegisterTriggerListener(trigger);
    }

    public override int CalculateHash(HashCalculator calc)
    {
      var hashcodes = _triggers.Select(calc.Calculate).ToList();
      hashcodes.Add(calc.Calculate(EffectFactory));
      hashcodes.Add(IsEnabled.GetHashCode());

      return HashCalculator.Combine(hashcodes);
    }

    protected virtual void Execute(object context)
    {
      if (!IsEnabled)
        return;

      if (TriggerOnlyIfOwningCardIsInPlay && OwningCard.Zone != Zone.Battlefield)
        return;

      var effect = EffectFactory.CreateEffect(this, triggerContext: context);

      if (TargetSelectors.Count > 0)
      {
        _decisions.EnqueueSetTriggeredAbilityTarget(
          OwningCard.Controller, effect, TargetSelectors);

        return;
      }

      if (UsesStack)
        Stack.Push(effect);
      else
        effect.Resolve();
    }

    private void RegisterTriggerListener(Trigger trigger)
    {
      trigger.Triggered += (o, e) => Execute(e.Context);
    }

    public void Disable()
    {
      IsEnabled = false;
    }

    public void Enable()
    {
      IsEnabled = true;
    }

    [Copyable]
    public class Factory : ITriggeredAbilityFactory
    {
      public Game Game { get; set; }
      public Action<TriggeredAbility> Init { get; set; }

      public TriggeredAbility Create(Card owningCard, Card sourceCard)
      {
        var ability = new TriggeredAbility();
        ability.OwningCard = owningCard;
        ability.SourceCard = sourceCard;
        ability._decisions = Game.Decisions;
        ability.Game = Game;
        ability._isEnabled = new Trackable<bool>(true, Game.ChangeTracker);

        Init(ability);

        return ability;
      }
    }
  }
}