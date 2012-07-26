namespace Grove.Core.Details.Cards.Effects
{
  using Ai;
  using Infrastructure;
  using Targeting;

  public interface IEffectSource : IHashable
  {
    EffectCategories EffectCategories { get; }
    Card OwningCard { get; }

    void EffectWasCountered();
    void EffectWasPushedOnStack();
    void EffectWasResolved();

    bool IsTargetStillValid(ITarget target, bool wasKickerPaid);
  }
}