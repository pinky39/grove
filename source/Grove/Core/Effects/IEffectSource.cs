namespace Grove.Core.Effects
{
  using Infrastructure;
  using Targeting;

  public interface IEffectSource : IHashable
  {
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectCountered(SpellCounterReason reason);
    void EffectPushedOnStack();
    void EffectResolved();

    bool IsTargetStillValid(ITarget target);
  }
}