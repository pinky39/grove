namespace Grove.Core.Ai
{
  using System.Collections.Generic;

  public class SearchResults
  {
    private readonly object _access = new object();
    private readonly Dictionary<int, InnerResult> _results = new Dictionary<int, InnerResult>();
        
    public bool NewResult(int id, bool isMax, out InnerResult searchResult)
    {
        lock (_access)
        {          
          if (_results.TryGetValue(id, out searchResult))
          {
            return true;
          }
          searchResult = new InnerResult(isMax);
          _results.Add(id , searchResult);          
        }
                        
        return false;
    }   

    public ISearchResult GetResult(int id)
    {
      InnerResult result;
      _results.TryGetValue(id, out result);
      return result;
    }
    
    public void Clear()
    {      
       _results.Clear();
    }
  }
}