namespace Grove.Gameplay.Card.Abilities
{
  using System.Collections.Generic;
  using Ai;
  using Characteristics;
  using Targeting;

  public class ActivationPrerequisites
  {
    public Card Card;
    public CardText Description;
    public int DistributeAmount;
    public int Index;
    public int MaxRepetitions = 1;
    public int? MaxX;
    public List<MachinePlayRule> Rules;
    public TargetSelector Selector;

    public bool HasXInCost { get { return MaxX.HasValue; } }
  }
}