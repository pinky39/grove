namespace Grove.Core.Effects
{
  using Grove.Infrastructure;
  using Grove.Core.Targeting;

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