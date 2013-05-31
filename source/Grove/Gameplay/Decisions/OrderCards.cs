namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using Results;

  public abstract class OrderCards : Decision<Ordering>
  {
    public List<Card> Cards;
    public IChooseDecisionResults<List<Card>, Ordering> ChooseDecisionResults;
    public IProcessDecisionResults<Ordering> ProcessDecisionResults;
    public string Title;

    protected override bool ShouldExecuteQuery { get { return Cards.Count > 1; } }

    protected override void SetResultNoQuery()
    {
      Result = Cards.Count == 0
        ? new Ordering()
        : new Ordering(0);
    }

    public override void ProcessResults()
    {
      ProcessDecisionResults.ProcessResults(Result);
    }
  }
}