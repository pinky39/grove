namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Card;
  using Results;
  using Targeting;
  using Zones;

  public abstract class SelectCards : Decision<ChosenCards>
  {
    public bool CanSelectOnlyCardsControlledByDecisionController = true;
    public IChooseDecisionResults<List<Card>, ChosenCards> ChooseDecisionResults;
    public int? MaxCount;
    public int MinCount;
    public Card OwningCard;
    public IProcessDecisionResults<ChosenCards> ProcessDecisionResults;
    public string Text;
    public string Instructions;

    public Zone Zone;

    private List<Card> _validCards;
    private ICardValidator _validator;

    protected SelectCards()
    {
      Result = null;
    }

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
        var count = ValidTargets.Count;
        return count > MinCount;
      }
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
      var result = Result ?? new ChosenCards(ValidTargets);
      ProcessDecisionResults.ProcessResults(result);
    }
  }
}