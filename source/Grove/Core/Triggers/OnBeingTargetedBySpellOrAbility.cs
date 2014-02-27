namespace Grove.Triggers
{
  using System;
  using System.Linq;
  using Grove.Events;
  using Grove.Infrastructure;

  public class OnBeingTargetedBySpellOrAbility : Trigger, IReceive<EffectPushedOnStack>
  {
    private readonly Func<ITarget, Effect, Trigger, bool> _predicate;

    private OnBeingTargetedBySpellOrAbility() {}

    public OnBeingTargetedBySpellOrAbility(Func<ITarget, Effect, Trigger, bool> predicate = null)
    {
      _predicate = predicate ?? ((target, effect, trigger) => target == trigger.Ability.OwningCard);
    }

    public void Receive(EffectPushedOnStack message)
    {
      var validTargets = message.Effect.Targets.Where(target => _predicate(target, message.Effect, this));

      foreach (var target in validTargets)
      {
        Set(message);
      }
    }
  }
}