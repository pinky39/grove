namespace Grove.Gameplay.Effects
{
  using System.Collections.Generic;
  using Infrastructure;
  using Targeting;

  public interface IEffectSource : IHashable
  {
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectCountered(SpellCounterReason reason);
    void EffectPushedOnStack();
    void EffectResolved();

    bool IsTargetStillValid(ITarget target, object triggerMessage = null);
    bool ValidateTargetDependencies(List<ITarget> costTargets, List<ITarget> effectTargets);
  }
}