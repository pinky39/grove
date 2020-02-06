namespace Grove.Effects
{
  using System.Collections.Generic;

  public interface IEffectSource
  {
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectCountered(SpellCounterReason reason);
    void EffectPushedOnStack();
    void EffectResolved(Effect.Context ctx);

    bool IsTargetStillValid(ITarget target, object triggerMessage = null);
    bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets);
  }
}