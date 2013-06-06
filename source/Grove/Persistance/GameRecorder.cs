namespace Grove.Persistance
{
  using System.IO;
  using Gameplay;

  public class GameRecorder
  {
    private readonly DecisionLog _decisionLog;
    private readonly Game _game;
    private readonly IdentityManager _identityManager;

    public GameRecorder(Game game, MemoryStream savedDecisions = null)
    {
      _game = game;
      _identityManager = new IdentityManager();
      _decisionLog = new DecisionLog(game, savedDecisions);
    }

    public bool IsPlayback { get { return !_decisionLog.IsAtTheEnd; } }

    public int CreateId(object obj)
    {
      if (_game.Ai.IsSearchInProgress)
        return -1;

      return _identityManager.GetId(obj);
    }

    public object GetObject(int id)
    {
      if (_game.Ai.IsSearchInProgress)
        return null;

      return _identityManager.GetObject(id);
    }

    public void SaveDecisionResult(object result)
    {
      if (_game.Ai.IsSearchInProgress)
        return;

      _decisionLog.SaveResult(result);
    }

    public object LoadDecisionResult()
    {
      return _decisionLog.LoadResult();
    }

    public SavedGame SaveGame()
    {
      var decisions = new MemoryStream();
      _decisionLog.WriteTo(decisions);

      var player1 = _game.Players.Player1;
      var player2 = _game.Players.Player2;

      var savedGame = new SavedGame
        {
          Player1 = new PlayerParameters
            {
              Name = player1.Name,
              Avatar = player1.Avatar,
              Deck = player1.Deck,
            },
          Player2 = new PlayerParameters
            {
              Name = player2.Name,
              Avatar = player2.Avatar,
              Deck = player2.Deck,
            },
          RandomSeed = _game.Random.Seed,
          Decisions = decisions,
          StateCount = _game.Turn.StateCount
        };

      return savedGame;
    }
  }
}