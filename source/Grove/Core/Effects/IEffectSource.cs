namespace Grove.Core.Effects
{
  using Ai;

  public interface IEffectSource
  {
    EffectCategories EffectCategories { get; }
    Card OwningCard { get; }
    void EffectWasCountered();
    void EffectWasPushedOnStack();
    void EffectWasResolved();
    bool IsTargetValid(ITarget target);
  }
}