namespace Grove.Core.Ai.CostRules
{
  using System;
  using Mana;
  using Targeting;

  public class CostRuleParameters
  {
    public Card OwningCard { get; private set; }
    public Targets Targets { get; private set; }    
    public Player Controller {get { return OwningCard.Controller; }}

    public CostRuleParameters(Card owningCard, Targets targets = null)
    {
      OwningCard = owningCard;
      Targets = targets;      
    }

    public int GetMaxConvertedMana(ManaUsage manaUsage)
    {
      var converted = Controller.GetConvertedMana(manaUsage);
      
      if (!OwningCard.IsPermanent)
        converted = converted - OwningCard.ConvertedCost;

      return converted;
    }
  }
}