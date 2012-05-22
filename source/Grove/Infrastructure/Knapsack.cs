namespace Grove.Infrastructure
{
  using System;
  using System.Collections.Generic;

  // 0-1 knapsack solver 
  // http://www.es.ele.tue.nl/education/5MC10/Solutions/knapsack.pdf  
  public static class Knapsack
  {
    public static List<KnapsackItem<T>> Solve<T>(IList<KnapsackItem<T>> items, int maxWeight)
    {
      var result = new List<KnapsackItem<T>>();

      if (items.Count == 0)
        return result;

      var scores = new int[items.Count + 1,maxWeight + 1];
      var keep = new int[items.Count + 1,maxWeight + 1];

      for (var curMaxWeight = 1; curMaxWeight <= maxWeight; curMaxWeight++)
      {
        for (var i = 0; i < items.Count; i++)
        {
          var scoreUpToCurrent = scores[i, curMaxWeight];
          var scoreIncludingCurrent = new Lazy<int>(() => scores[i, curMaxWeight - items[i].Weight] + items[i].Value);

          if (items[i].Weight <= curMaxWeight && scoreUpToCurrent < scoreIncludingCurrent.Value)
          {
            scores[i + 1, curMaxWeight] = scoreIncludingCurrent.Value;
            keep[i + 1, curMaxWeight] = 1;
          }
          else
          {
            scores[i + 1, curMaxWeight] = scoreUpToCurrent;
            keep[i + 1, curMaxWeight] = 0;
          }
        }
      }

      var curWeight = maxWeight;

      for (var i = items.Count - 1; i >= 0; i--)
      {
        if (keep[i + 1, curWeight] == 1)
        {
          result.Add(items[i]);
          curWeight -= items[i].Weight;
        }
      }

      return result;
    }
  }
}