namespace Grove.Tests.Unit
{
  using System.Linq;
  using Grove.Infrastructure;
  using Xunit;

  public class KnapsackFacts
  {
    [Fact]
    public void Solve1()
    {
      var items = new[]{
        new KnapsackItem<string>("Shoes", weight: 30, value: 50),
        new KnapsackItem<string>("Gloves", weight: 5, value: 5),
        new KnapsackItem<string>("Belt", weight: 10, value: 12),
        new KnapsackItem<string>("Helmet", weight: 50, value: 100),
      };

      var valueables = Knapsack.Solve(items, 95);

      var ordered = valueables
        .OrderByDescending(x => x.Value)
        .Select(x => x.Item)
        .ToArray();
        

      Assert.Equal( new[] {"Helmet", "Shoes", "Belt", "Gloves"}, ordered);
    }

    [Fact]
    public void Solve2()
    {
      var items = new[]{
        new KnapsackItem<string>("Item1", weight: 10, value: 60),
        new KnapsackItem<string>("Item2", weight: 20, value: 100),
        new KnapsackItem<string>("Item3", weight: 30, value: 120),        
      };

      var valueables = Knapsack.Solve(items, 50);

      var ordered = valueables
        .OrderByDescending(x => x.Value)
        .Select(x => x.Item)
        .ToArray();


      Assert.Equal(new[] { "Item3", "Item2" }, ordered);
    }
  }
}