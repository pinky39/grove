namespace Grove.Core
{
  using Ai;

  public class SpellPrerequisites
  {
    public bool TargetsSelf { get; set; }
    public bool CanBeSatisfied { get; set; }
    public bool CanCastWithKicker { get; set; }
    public TargetSelector CostTargetSelector { get; set; }
    public CardText Description { get; set; }
    public TargetSelector EffectTargetSelector { get; set; }
    public bool HasXInCost { get { return MaxX != null; } }
    public bool IsManaSource { get; set; }
    public TimingDelegate Timming { get; set; }
    public int? MaxX { get; set; }
    public bool NeedsCostTargets { get { return CostTargetSelector != null; } }
    public bool NeedsEffectTargets { get { return EffectTargetSelector != null; } }
    public bool NeedsKickerEffectTargets { get { return KickerTargetSelector != null; } }
    public CalculateX XCalculator { get; set; }
    public TargetSelector KickerTargetSelector { get; set; }
  }
}