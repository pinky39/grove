namespace Grove.Artifical.Decisions
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Decisions.Results;
  using Infrastructure;

  public class PlaySpellOrAbility : Gameplay.Decisions.PlaySpellOrAbility, ISearchNode, IDecisionExecution
  {
    private readonly DecisionExecutor _executor;
    private List<IPlayable> _playables;

    public PlaySpellOrAbility()
    {
      Result = new ChosenPlayable {Playable = new Pass()};
      _executor = new DecisionExecutor(this);
    }

    public override bool HasCompleted { get { return _executor.HasCompleted; } }
    bool IDecisionExecution.ShouldExecuteQuery { get { return ShouldExecuteQuery; } }

    void IDecisionExecution.ExecuteQuery()
    {
      ExecuteQuery();
    }

    Game ISearchNode.Game { get { return Game; } }

    public int ResultCount { get { return _playables.Count; } }

    public void GenerateChoices()
    {
      _playables = GeneratePlayables();

      // consider passing priority every time
      _playables.Add(new Pass());
    }

    public void SetResult(int index)
    {
      Result = new ChosenPlayable {Playable = _playables[index]};
      LogFile.Debug("Move is {0}", _playables[index]);
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

    private List<IPlayable> GeneratePlayables()
    {
      if (Stack.TopSpellOwner == Controller || (Ai.IsSearchInProgress && Turn.StepCount > Ai.PlaySpellsUntilDepth))
      {
        // if you own the top spell just pass so it resolves
        // you will get priority again when it resolves
        return new List<IPlayable>();
      }

      return new PlayableGenerator(Controller, Game).GetPlayables();
    }

    public override string ToString()
    {
      return string.Format("{0}, {1} plays", Game.Turn.Step, Controller);
    }
  }
}