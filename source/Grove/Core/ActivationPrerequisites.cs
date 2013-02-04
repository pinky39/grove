namespace Grove.Core
{
  using System.Collections.Generic;
  using Ai;
  using Targeting;

  public class ActivationPrerequisites
  {    
    public int Index;
    public CardText Description;    
    public List<MachinePlayRule> Rules;
    public Card Card;    
    public int? MaxX;
    public TargetSelector Selector;
    public int DistributeAmount;
  }
}