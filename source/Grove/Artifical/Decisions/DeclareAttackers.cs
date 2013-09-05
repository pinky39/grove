namespace Grove.Artifical.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Decisions.Results;

  public class DeclareAttackers : Gameplay.Decisions.DeclareAttackers, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;
    private List<List<Card>> _declarations;

    public DeclareAttackers()
    {
      Result = new ChosenCards();
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
      _declarations = GetAttackersDeclarations();
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
    
    private List<List<Card>> GetAttackersDeclarations()
    {
      var results = new List<List<Card>>();
      
      var noAttackers = new List<Card>();
      
      results.Add(noAttackers);      

      var allAttackers = Controller.Battlefield.CreaturesThatCanAttack.ToList();

      if (allAttackers.Count > 0)
      {
        var parameters = new AttackStrategyParameters
          {
            AttackerCandidates = allAttackers,
            BlockerCandidates = Defender.Battlefield.CreaturesThatCanBlock.ToList(),
            DefendingPlayersLife = Defender.Life
          };

        var chosenAttackers = new AttackStrategy(parameters).ChooseAttackers();

        if (chosenAttackers.Count > 0)
        {          
          results.Add(chosenAttackers);          
        }

        if (chosenAttackers.Count < allAttackers.Count)
        {          
          results.Add(allAttackers);
        }
      }

      return results;
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} declares attackers", Game.Turn.Step, Controller);
    }
  }
}