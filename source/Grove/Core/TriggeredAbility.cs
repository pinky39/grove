namespace Grove.Core
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Controllers;
  using Infrastructure;
  using Triggers;
  using Zones;

  [Copyable]
  public class TriggeredAbility : Ability, IDisposable, ICopyContributor
  {
    private readonly List<Trigger> _triggers = new List<Trigger>();
    private Decisions _decisions;

    public TriggeredAbility()
    {
      UsesStack = true;
    }

    public bool TriggerOnlyIfOwningCardIsInPlay { get; set; }

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

      return HashCalculator.Combine(hashcodes);
    }

    protected virtual void Execute(object context)
    {
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