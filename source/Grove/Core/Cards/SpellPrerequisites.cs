namespace Grove.Core.Cards
{
  using Grove.Core.Ai;
  using Grove.Core.Targeting;

  public class SpellPrerequisites
  {
    public static readonly SpellPrerequisites CannotBeSatisfied = new SpellPrerequisites {CanBeSatisfied = false};

    public SpellPrerequisites()
    {
      TargetSelector = TargetSelector.NullSelector;
      KickerTargetSelector = TargetSelector.NullSelector;
    }

    public bool IsAbility { get; set; }
    public bool TargetsSelf { get; set; }
    public bool CanBeSatisfied { get; set; }
    public bool CanCastWithKicker { get; set; }
    public TargetSelector TargetSelector { get; set; }
    public CardText Description { get; set; }
    public bool HasXInCost { get { return MaxX != null; } }
    public TimingDelegate Timing { get; set; }
    public int? MaxX { get; set; }
    public CalculateX XCalculator { get; set; }
    public TargetSelector KickerTargetSelector { get; set; }
    public bool NeedsKickerTargets { get { return KickerTargetSelector.Count > 0; } }
    public bool NeedsTargets { get { return TargetSelector.Count > 0; } }
    public bool IsSpell { get { return !IsAbility; } }
    public bool DistributeDamage { get; set; }
  }
}