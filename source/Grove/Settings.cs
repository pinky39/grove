using Grove.AI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Grove
{
  public class Settings
  {
    private static Settings _global;
        
    // This should be used only when access to Game context is not possible.
    public static Settings Readonly
    {
      get
      {
        if (_global == null)
        {
          _global = Settings.Load();
        }
        
        return _global;
      }
    }
    public int BasicLandVersions = 4;

    public AiConfiguration Ai = new AiConfiguration
    {
      SearchDepth = 60,
      TargetCount = 2,
#if DEBUG
      Strategy = MultithreadStrategy.SingleThreaded
#else
      Strategy = MultithreadStrategy.MultiThreaded2
#endif
    };

    public class AiConfiguration
    {
      public int SearchDepth;
      public int TargetCount;
      public MultithreadStrategy Strategy;
    }

    public enum MultithreadStrategy
    {
      SingleThreaded,
      MultiThreaded1,
      MultiThreaded2
    }   
    
    public SearchParameters GetSearchParameters()
    {
      return new SearchParameters(Ai.SearchDepth, Ai.TargetCount, GetPartitioningStrategy());
    }

    private SearchPartitioningStrategy GetPartitioningStrategy()
    {
      switch (Ai.Strategy)
      {
        case (MultithreadStrategy.MultiThreaded1):
          return SearchPartitioningStrategies.MultiThreaded1;
        case (MultithreadStrategy.MultiThreaded2):
          return SearchPartitioningStrategies.MultiThreaded2;
      }

      return SearchPartitioningStrategies.SingleThreaded;
    }

    public static Settings Load()
    {
      const string filename = "settings.json";

      if (File.Exists(filename))
      {
        return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filename));
      }

      return new Settings();
    }
  }
}