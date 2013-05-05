namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using Card;
  using Results;

  public abstract class OrderCards : Decision<Ordering>
  {
    public IChooseDecisionResults<Ordering> ChooseDecisionResults;
    public IProcessDecisionResults<Ordering> ProcessDecisionResults;
    public List<Card> Cards;
    public string Message;
        
    protected override bool ShouldExecuteQuery
    {
      get { return Cards.Count > 1; }
    }

    public override void ProcessResults()
    {
      switch (Cards.Count)
      {
        case 0:
          Result = new Ordering();
          break;
        case 1:
          Result = new Ordering(0);
          break;
      }
                  
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}