namespace Grove.Core.Ai
{
  using System.Collections.Generic;
  using Targeting;

  public class ActivationContext
  {
    public Card Card;
    public bool DistributeDamage;
    public int? MaxX;
    public TargetSelector Selector;
    public bool CanCancel = true;

    public List<Targets> Targets;
    public int? X;
    public bool CancelActivation;
    

    public ActivationContext() {}

    public ActivationContext(ActivationPrerequisites prerequisites)
    {
      Card = prerequisites.Card;
      DistributeDamage = prerequisites.DistributeDamage;
      MaxX = prerequisites.MaxX;
      Selector = prerequisites.Selector;
    }
  }
}