namespace Grove
{
  using System.Collections.Generic;
  using AI;

  public class ActivationPrerequisites
  {
    public bool CanBePlayedRegardlessofTime = true;
    public bool CanBePlayedAtThisTime;
    public bool CanBePayed;    
    public Card Card;
    public CardText Description;
    public int DistributeAmount;
    public int Index;
    public int MaxRepetitions;    
    public int? MaxXIfCastingCostIsNotPayed;
    public int? MaxX;    
    public List<MachinePlayRule> Rules;
    public TargetSelector Selector;

    public bool HasXInCost { get { return MaxX.HasValue; } }
    
    public bool CanBePlayed
    {
      get
      {
        return CanBePlayedAtThisTime &&
          CanBePlayedRegardlessofTime;
      }
    }    

    public bool CanBePlayedAndPayed
    {
      get { return CanBePlayed && CanBePayed; }
    }    
  }
}