namespace Grove.Core.Decisions.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Ai;
  using Results;

  public class DeclareAttackers : Decisions.DeclareAttackers, ISearchNode, IDecisionExecution
  {
    private List<List<Card>> _declarations;
    private DecisionExecutor _executor;

    public DeclareAttackers()
    {
      Result = FinalResult();
    }

    private Player Defender { get { return Game.Players.GetOpponent(Controller); } }
    public override bool HasCompleted { get { return _executor.HasCompleted; } }

    public Search Search { get { return Game.Search; } }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    public int ResultCount { get { return _declarations.Count; } }

    public void GenerateChoices()
    {
      _declarations = GetAttackersDeclarations().ToList();
    }

    public void SetResult(int index)
    {
      Result = _declarations[index];
    }

    public override void Init()
    {
      _executor = new DecisionExecutor(this, Game.ChangeTracker);
    }

    public override void Execute()
    {
      _executor.Execute();
    }

    protected override void ExecuteQuery()
    {
      Search.SetBestResult(this);
    }

    private static ChosenCards FinalResult()
    {
      // this is used when search is stoped when search depth is 
      // reached 
      return new ChosenCards();
    }

    private IEnumerable<List<Card>> GetAttackersDeclarations()
    {
      // none
      yield return new List<Card>();

      var allAttackers = Controller.Battlefield.CreaturesThatCanAttack.ToList();

      // quick heuristic
      yield return new AttackStrategy(
        Controller.Life,
        Defender.Life,
        allAttackers,
        Defender.Battlefield.CreaturesThatCanBlock).ToList();


      yield return allAttackers;
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} declares attackers", Game.Turn.Step, Controller);
    }
  }
}