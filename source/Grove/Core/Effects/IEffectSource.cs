namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Ai;

  public interface IEffectSource : IHashable
  {
    EffectCategories EffectCategories { get; }
    Card OwningCard { get; }
    void EffectWasCountered();
    void EffectWasPushedOnStack();
    void EffectWasResolved();
    bool AreTargetsStillValid(IList<ITarget> targets, bool wasKickerPaid);
  }
}