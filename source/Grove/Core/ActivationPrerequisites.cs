namespace Grove.Core
{
  using Ai;
  using Targeting;

  public class ActivationPrerequisites
  {    
    public int Index;
    public CardText Description;    
    public MachinePlayRule[] Rules;
    public Card Card;
    public bool DistributeDamage;
    public int? MaxX;
    public TargetSelector Selector;
  }
}