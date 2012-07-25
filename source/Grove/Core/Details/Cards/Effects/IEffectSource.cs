namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
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
    
    bool AreTargetsStillValid(IList<Target> targets, bool wasKickerPaid);
  }
}