namespace Grove.Core.Cards
{
  using Ai;
  using Targeting;

  public class SpellPrerequisites
  {
    public static readonly SpellPrerequisites CannotBeSatisfied = new SpellPrerequisites {CanBeSatisfied = false};

    public SpellPrerequisites()
    {
      TargetSelector = TargetSelector.NullSelector;
    }
    
    public bool CanBeSatisfied { get; set; }    
    public TargetSelector TargetSelector { get; set; }
    public CardText Description { get; set; }
    public bool HasXInCost { get { return MaxX != null; } }
    public TimingDelegate Timing = delegate { return true; };
    public int? MaxX { get; set; }
    public CalculateX XCalculator { get; set; }
    public bool RequiresTargets { get { return TargetSelector.RequiresTargets; } }    
    public bool DistributeDamage { get; set; }
  }
}