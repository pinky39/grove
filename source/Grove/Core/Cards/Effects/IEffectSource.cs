namespace Grove.Core.Cards.Effects
{
  using Ai;
  using Infrastructure;
  using Targeting;

  public interface IEffectSource : IHashable
  {
    EffectCategories EffectCategories { get; }
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectCountered(SpellCounterReason reason);
    void EffectPushedOnStack();
    void EffectResolved();

    bool IsTargetStillValid(ITarget target, bool wasKickerPaid);
  }
}