namespace Grove.Ai.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay;
  using Gameplay.Card;
  using Gameplay.Decisions.Results;
  using Gameplay.Player;

  public class DeclareAttackers : Gameplay.Decisions.DeclareAttackers, ISearchNode, IDecisionExecution
  {
    private List<List<Card>> _declarations;
    private readonly DecisionExecutor _executor;

    public DeclareAttackers()
    {
      Result = FinalResult();
      _executor = new DecisionExecutor(this);
    }

    private Player Defender { get { return Controller.Opponent; } }
    public override bool HasCompleted { get { return _executor.HasCompleted; } }

    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    Game ISearchNode.Game { get { return Game; } }

    public int ResultCount { get { return _declarations.Count; } }

    public void GenerateChoices()
    {
      _declarations = GetAttackersDeclarations().ToList();
    }

    public void SetResult(int index)
    {
      Result = _declarations[index];
    }

    public override void Initialize(Player controller, Game game)
    {
      base.Initialize(controller, game);
      _executor.Initialize(game.ChangeTracker);
    }

    public override void Execute()
    {
      _executor.Execute();
    }

    protected override void ExecuteQuery()
    {            
      Ai.SetBestResult(this);
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