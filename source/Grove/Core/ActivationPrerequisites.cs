namespace Grove
{
  using System;
  using System.Collections.Generic;
  using AI;

  public class ActivationPrerequisites
  {
    public bool CanPay;
    public Card Card;
    public CardText Description;
    public int DistributeAmount;
    public int Index;
    public int MaxRepetitions;
    public int? MaxX;
    public List<MachinePlayRule> Rules;
    public TargetSelector Selector;

    public bool HasXInCost
    {
      get { return MaxX.HasValue; }
    }
  }
}