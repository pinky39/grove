namespace Grove.Triggers
{
  using System;
  using System.Linq;
  using Events;
  using Infrastructure;

  public class OnBeingTargetedBySpellOrAbility : Trigger, IReceive<EffectPutOnStackEvent>
  {
    private readonly Func<ITarget, Effect, Trigger, bool> _predicate;

    private OnBeingTargetedBySpellOrAbility() {}

    public OnBeingTargetedBySpellOrAbility(Func<ITarget, Effect, Trigger, bool> predicate = null)
    {
      _predicate = predicate ?? ((target, effect, trigger) => target == trigger.Ability.OwningCard);
    }

    public void Receive(EffectPutOnStackEvent message)
    {
      var effectTargets = message.Effect.Targets
        .Where(target => _predicate(target, message.Effect, this))
        .ToList();

      foreach (var target in effectTargets)
      {
        Set(message);
      }
    }
  }
}