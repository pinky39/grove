﻿namespace Grove.Ai
{
  using System.Collections.Generic;

  public class SearchResults
  {
    private readonly object _access = new object();
    private readonly Dictionary<int, InnerResult> _results = new Dictionary<int, InnerResult>();

    public bool NewResult(int gameState, bool isMax, int stepCount, out InnerResult searchResult)
    {            
      lock (_access)
      {
        if (_results.TryGetValue(gameState, out searchResult))
        {
          return true;
        }
        
        searchResult = new InnerResult(
          id: gameState, 
          isMax: isMax, 
          stepCount: stepCount);
        
        _results.Add(gameState, searchResult);
      }

      return false;
    }
     

    public ISearchResult GetResult(int gameState)
    {
      InnerResult result;      
      _results.TryGetValue(gameState, out result);
      return result;
    }

    public void Clear()
    {
      _results.Clear();
    }
  }
}