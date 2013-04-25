namespace Grove.Ai
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Gameplay;
  using Gameplay.Messages;

  public class GameAi
  {
    private readonly Game _game;
    private readonly Queue<int> _searchDurations = new Queue<int>(new[] {0});
    private readonly SearchParameters _searchParameters;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    private SearchResults _cachedResults = new SearchResults();
    private Search _currentSearch;

    public GameAi(SearchParameters searchParameters, Game game)
    {
      _game = game;
      _searchParameters = searchParameters;
    }

    public bool IsSearchInProgress { get { return _currentSearch != null; } }
    public SearchParameters Parameters { get { return _searchParameters; } }
    public int PlaySpellsUntilDepth { get { return _game.Turn.GetStepCountAtNextTurnCleanup(); } }
    public event EventHandler SearchStarted = delegate { };
    public event EventHandler SearchFinished = delegate { };
    public SearchStatistics LastSearchStatistics { get; private set; }

    public void SetBestResult(ISearchNode searchNode)
    {
      searchNode.GenerateChoices();

      if (searchNode.ResultCount == 0)
      {
        return;
      }
      
      if (searchNode.ResultCount == 1)
      {
        searchNode.SetResult(0);
        return;
      }

      if (IsSearchInProgress)
      {        
        _currentSearch.EvaluateNode(searchNode);
        return;
      }

      // searching player must be set before calculating the
      // hash, to only get cached results by that player
      _game.Players.Searching = searchNode.Controller;        
      var cached = _cachedResults.GetResult(_game.CalculateHash());

      var result = cached == null ? 
        StartNewSearch(searchNode) : 
        cached.BestMove.GetValueOrDefault();

      searchNode.SetResult(result);
    }

    private int StartNewSearch(ISearchNode searchNode)
    {
      _searchParameters.AdjustPerformance(_searchDurations);

      _currentSearch = new Search(_searchParameters,
        searchNode.Controller, _game);

      SearchStarted(this, EventArgs.Empty);
      
      _game.Publish(new SearchStarted
        {
          SearchDepthLimit = _searchParameters.SearchDepth,
          TargetCountLimit = _searchParameters.TargetCount
        });

      StartTiming();

      // execute search
      _currentSearch.Start(searchNode);

      StopTiming();

      _cachedResults = _currentSearch.ResultsCache;
      var result = _currentSearch.Result;

      LastSearchStatistics = _currentSearch.GetSearchStatistics();

      _game.Publish(new SearchFinished());
      SearchFinished(this, EventArgs.Empty);

      _currentSearch = null;
      return result;
    }

    private void StopTiming()
    {
      _stopwatch.Stop();

      if (_searchDurations.Count == 10)
      {
        _searchDurations.Dequeue();
      }

      _searchDurations.Enqueue(((int) _stopwatch.Elapsed.TotalMilliseconds));
      _stopwatch.Reset();
    }

    private void StartTiming()
    {
      _stopwatch.Start();
    }
  }
}