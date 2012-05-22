namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using Controllers;
  using Infrastructure;
  using Triggers;

  [Copyable]
  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly List<Trigger> _triggers = new List<Trigger>();
    private Decisions _decisions;

    public TriggeredAbility()
    {
      UsesStack = true;
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

    public void AddTrigger(ITriggerFactory triggerFactory)
    {
      var trigger = triggerFactory.CreateTrigger(this);
      _triggers.Add(trigger);
      RegisterTriggerListener(trigger);
    }

    public override int CalculateHash(HashCalculator hashCalculator)
    {
      return hashCalculator.Calculate(
        _triggers,
        EffectFactory);
    }

    protected virtual void Execute()
    {
      var effect = EffectFactory.CreateEffect(this);      
      
      if (NeedsTarget)
      {
        _decisions.EnqueueSetTriggeredAbilityTarget(
          OwningCard.Controller, effect, TargetSelector);

        return;
      }

      if (UsesStack)
        Stack.Push(effect);
      else 
        effect.Resolve();
    }

    private void RegisterTriggerListener(Trigger trigger)
    {
      trigger.Triggered += (o, e) => Execute();
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
        
        Init(ability);

        return ability;
      }
    }
  }
}