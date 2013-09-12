namespace Grove.Gameplay.Abilities
{
  using System;
  using System.Collections.Generic;
  using Artifical;
  using Characteristics;
  using Targeting;

  public class ActivationPrerequisites
  {
    public Card Card;
    public CardText Description;
    public int DistributeAmount;
    public int Index;
    public Lazy<int> MaxRepetitions;
    public Lazy<int?> MaxX;
    public List<MachinePlayRule> Rules;
    public TargetSelector Selector;
    public Lazy<bool> CanPay;
    public bool HasXInCost { get { return MaxX.Value.HasValue; } }
  }
}