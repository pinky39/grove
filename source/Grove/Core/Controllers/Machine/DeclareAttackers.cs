namespace Grove.Core.Controllers.Machine
{
  using System.Collections.Generic;
  using System.Linq;
  using Ai;
  using Infrastructure;
  using Results;

  public class DeclareAttackers : Controllers.DeclareAttackers, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;
    private List<List<Card>> _declarations;

    private DeclareAttackers() {}

    public DeclareAttackers(ChangeTracker changeTracker)
    {
      _executor = new DecisionExecutor(this, changeTracker);
      Result = FinalResult();
    }

    private Player Defender { get { return Game.Players.GetOpponent(Player); } }
    public override bool HasCompleted { get { return _executor.HasCompleted; } }

    public Search Search { get; set; }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    public Game Game { get; set; }

    public int ResultCount { get { return _declarations.Count; } }

    public void GenerateChoices()
    {
      _declarations = GetAttackersDeclarations().ToList();
    }

    public void SetResult(int index)
    {
      Result = _declarations[index];
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

      var allAttackers = Player.Battlefield.CreaturesThatCanAttack.ToList();

      // quick heuristic
      yield return new AttackStrategy(
        Player.Life,
        Defender.Life,
        allAttackers,
        Defender.Battlefield.CreaturesThatCanBlock).ToList();


      yield return allAttackers;
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} declares attackers", Game.Turn.Step, Player);
    }
  }
}