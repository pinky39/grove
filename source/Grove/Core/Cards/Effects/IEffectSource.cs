namespace Grove.Core.Cards.Effects
{
  using Targeting;

  public interface IEffectSource
  {
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectCountered(SpellCounterReason reason);
    void EffectPushedOnStack();
    void EffectResolved();

    bool IsTargetStillValid(ITarget target, bool wasKickerPaid);
  }
}