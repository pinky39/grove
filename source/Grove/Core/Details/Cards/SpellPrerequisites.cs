namespace Grove.Core.Details.Cards
{
  using Ai;
  using Targeting;

  public class SpellPrerequisites
  {
    public SpellPrerequisites()
    {
      TargetSelector = new TargetSelector();
      KickerTargetSelector = new TargetSelector();
    }

    public bool IsAbility { get; set; }
    public bool TargetsSelf { get; set; }
    public bool CanBeSatisfied { get; set; }
    public bool CanCastWithKicker { get; set; }
    public TargetSelector TargetSelector { get; set; }
    public CardText Description { get; set; }
    public bool HasXInCost { get { return MaxX != null; } }
    public bool IsManaSource { get; set; }
    public TimingDelegate Timming { get; set; }
    public int? MaxX { get; set; }
    public CalculateX XCalculator { get; set; }
    public TargetSelector KickerTargetSelector { get; set; }
    public bool NeedsKickerTargets { get { return KickerTargetSelector.Count > 0; } }
    public bool NeedsTargets { get { return TargetSelector.Count > 0; } }
    public bool IsSpell { get { return !IsAbility; } }
    public bool DistributeDamage { get; set; }
  }
}