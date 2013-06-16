namespace Grove.Artifical
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Threading;
  using Gameplay;
  using Gameplay.Debuging;
  using Gameplay.Messages;
  using Infrastructure;

  public class SearchRunner
  {
    private readonly Game _game;
    private readonly SearchResults _player1Results = new SearchResults();
    private readonly SearchResults _player2Results = new SearchResults();
    private readonly Queue<int> _searchDurations = new Queue<int>(new[] {0});
    private readonly SearchParameters _searchParameters;
    private Search _currentSearch;

    public SearchRunner(SearchParameters searchParameters, Game game)
    {
      _game = game;
      _searchParameters = searchParameters;
    }

    public bool IsSearchInProgress { get { return _currentSearch != null; } }
    public SearchParameters Parameters { get { return _searchParameters; } }
    public int PlaySpellsUntilDepth { get { return _game.Turn.GetStepCountAtNextTurnCleanup(); } }
    public SearchStatistics LastSearchStatistics { get; private set; }

    private void SearchMonitor(object state)
    {
      if (IsSearchInProgress == false)
        return;

      if (_currentSearch.Duration.TotalSeconds > 5)
      {
        GenearateScenario();
      }
    }

    public event EventHandler SearchStarted = delegate { };
    public event EventHandler SearchFinished = delegate { };

    public void SetBestResult(ISearchNode searchNode)
    {      
      // set searching player the first time
      if (!IsSearchInProgress)
        searchNode.Game.Players.Searching = searchNode.Controller;
      
      // ask search node to generate all choices
      searchNode.GenerateChoices();

      // Zero choices can happen if there are no legal targets
      // of a triggered ability.
      if (searchNode.ResultCount == 0)
      {
        return;
      }

      // Only one choice, nothing to consider here.
      if (searchNode.ResultCount == 1)
      {
        searchNode.SetResult(0);
        return;
      }


      if (IsSearchInProgress)
      {
        // More than one choice, and the search is already in progress.
        // Expand the tree by evaluating each branch.
        _currentSearch.EvaluateNode(searchNode);
        return;
      }

      // More than one choice, find the best one.
      int bestChoice;

      // First try cached result from previous search.
      // If no results are found start a new search.            
      var cachedResults = GetCachedResults(searchNode.Controller);
      var cached = cachedResults.GetResult(_game.CalculateHash());
                  
      if (cached == null)
      {
        bestChoice = StartNewSearch(searchNode, cachedResults);
      }      
      else
      {        
        bestChoice = cached.BestMove.GetValueOrDefault();

        if (cached.ChildrenCount != searchNode.ResultCount)
        {
          // cache is not ok, try to recover by new search
          // write debug report, so this error can be reproduced.
          
          LogFile.Error("Invalid cached result, cached result count is {0} node's is {1}.", 
            cached.ChildrenCount, searchNode.ResultCount);
          
          _game.WriteDebugReport();
          bestChoice = StartNewSearch(searchNode, cachedResults);
        }
      }

      searchNode.SetResult(bestChoice);
    }

    private SearchResults GetCachedResults(Player player)
    {
      if (player == _game.Players.Player1)
        return _player1Results;

      return _player2Results;
    }

    private int StartNewSearch(ISearchNode searchNode, SearchResults cachedResults)
    {
      _searchParameters.AdjustPerformance(_searchDurations);
      ClearResultCache();

      _currentSearch = new Search(_searchParameters,
        searchNode.Controller, cachedResults, _game);

      SearchStarted(this, EventArgs.Empty);

      _game.Publish(new SearchStarted
        {
          SearchDepthLimit = _searchParameters.SearchDepth,
          TargetCountLimit = _searchParameters.TargetCount
        });

      using (new Timer(SearchMonitor, null, 0, 2000))
      {
        LastSearchStatistics = _currentSearch.Start(searchNode);
      }

      var result = _currentSearch.Result;

      UpdateSearchDurations();

      _game.Publish(new SearchFinished());
      SearchFinished(this, EventArgs.Empty);

      _currentSearch = null;
      return result;
    }

    private void ClearResultCache()
    {
      _player1Results.Clear();
      _player2Results.Clear();
    }

    private void UpdateSearchDurations()
    {
      if (_searchDurations.Count == 10)
      {
        _searchDurations.Dequeue();
      }
      _searchDurations.Enqueue(((int) LastSearchStatistics.Elapsed.TotalMilliseconds));
    }

    [Conditional("DEBUG")]
    private void GenearateScenario()
    {
      var scenarioGenerator = new ScenarioGenerator(_game);
      LogFile.Info(scenarioGenerator.WriteScenarioToString());
    }
  }
}