namespace Grove.Core.Cards
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;
  using Infrastructure;
  using Targeting;
  using Triggers;
  using Zones;

  [Copyable]
  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly List<Trigger> _triggers = new List<Trigger>();
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
      var trigger = triggerFactory.CreateTrigger(this, Game);
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

    protected virtual void Execute(object triggerMessage)
    {
      if (!IsEnabled)
        return;

      if (TriggerOnlyIfOwningCardIsInPlay && OwningCard.Zone != Zone.Battlefield)
        return;

      if (TargetSelector.Count > 0)
      {
        Game.Enqueue<SetTriggeredAbilityTarget>(
          controller: OwningCard.Controller,
          init: p =>
            {
              p.Source = this;
              p.Factory = EffectFactory;
              p.Trigger = triggerMessage;
              p.TargetSelector = TargetSelector;
            });

        return;
      }

      var effectParameters = new EffectParameters(this, EffectCategories,
        triggerMessage: triggerMessage);

      var effect = EffectFactory.CreateEffect(effectParameters, Game);

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

    public class Factory : ITriggeredAbilityFactory
    {
      public Action<TriggeredAbility> Init { get; set; }

      public TriggeredAbility Create(Card owningCard, Card sourceCard, Game game)
      {
        var ability = new TriggeredAbility();
        ability.OwningCard = owningCard;
        ability.SourceCard = sourceCard;
        ability.Game = game;
        ability._isEnabled = new Trackable<bool>(true, game.ChangeTracker);

        Init(ability);

        return ability;
      }
    }
  }
}