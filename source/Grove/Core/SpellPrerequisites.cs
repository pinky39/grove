namespace Grove.Core
{
  using Ai;

  public class SpellPrerequisites
  {    
    public SpellPrerequisites()
    {
      TargetSelectors = new TargetSelectors();  
    }
    
    public bool TargetsSelf { get; set; }
    public bool CanBeSatisfied { get; set; }
    public bool CanCastWithKicker { get; set; }
    public TargetSelectors TargetSelectors { get; set; }
    public CardText Description { get; set; }
    public bool HasXInCost { get { return MaxX != null; } }
    public bool IsManaSource { get; set; }
    public TimingDelegate Timming { get; set; }
    public int? MaxX { get; set; }
    public CalculateX XCalculator { get; set; }    
  }
}