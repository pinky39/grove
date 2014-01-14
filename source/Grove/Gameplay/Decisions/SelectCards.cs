namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Results;
  using Targeting;
  using Zones;

  public abstract class SelectCards : Decision<ChosenCards>
  {
    public bool CanSelectOnlyCardsControlledByDecisionController = true;
    public IChooseDecisionResults<List<Card>, ChosenCards> ChooseDecisionResults;
    public string Instructions;
    public int? MaxCount;
    public int MinCount;
    public bool AurasNeedTarget;
    public Card OwningCard;
    public IProcessDecisionResults<ChosenCards> ProcessDecisionResults;
    public string Text;

    public Zone Zone;

    private List<Card> _validCards;
    private ICardValidator _validator;    

    protected List<Card> ValidTargets
    {
      get
      {
        return
          _validCards ?? (_validCards =
            GenerateTargets((zone, owner) =>
              {
                if (CanSelectOnlyCardsControlledByDecisionController && owner != Controller)
                  return false;

                return zone == Zone;
              })
              .Where(x => x.IsCard())
              .Select(x => x.Card())
              .Where(IsValidCard).ToList());
      }
    }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        if (MaxCount == 0)
          return false;

        return true;                
      }
    }

    protected override void SetResultNoQuery()
    {
      Result = new ChosenCards(ValidTargets.Take(MinCount));
    }

    public void Validator(ICardValidator validator)
    {
      _validator = validator;
    }

    public void Validator(Func<Card, bool> validator)
    {
      _validator = new DelegateCardValidator(validator);
    }

    public bool IsValidCard(Card card)
    {
      return _validator.IsValidCard(card);
    }

    public override void ProcessResults()
    {      
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}