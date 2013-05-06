namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using Results;

  public abstract class OrderCards : Decision<Ordering>
  {
    public List<Card> Cards;
    public IChooseDecisionResults<Ordering> ChooseDecisionResults;
    public string Message;
    public IProcessDecisionResults<Ordering> ProcessDecisionResults;

    protected override bool ShouldExecuteQuery { get { return Cards.Count > 1; } }

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