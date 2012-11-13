namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Ai;
  using Grove.Infrastructure;
  using Grove.Core.Targeting;

  public interface IEffectSource : IHashable
  {
    EffectCategories EffectCategories { get; }
    Card OwningCard { get; }
    Card SourceCard { get; }

    void EffectWasCountered();
    void EffectWasPushedOnStack();
    void EffectWasResolved();

    bool IsTargetStillValid(ITarget target, bool wasKickerPaid);
  }
}