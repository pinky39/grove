namespace Grove.Tests.Unit
{
  using Grove.Infrastructure;
  using Xunit;

  public class SetExFacts
  {
    [Fact]
    public void SubsetsOfSize3()
    {
      var set = new[] {1, 2, 3, 4, 5};
      var subsets = set.KSubsets(3).ToArray();

      var expected = new[]
        {
          new[] {1, 2, 3},
          new[] {1, 2, 4},
          new[] {1, 2, 5},
          new[] {1, 3, 4},
          new[] {1, 3, 5},
          new[] {1, 4, 5},
          new[] {2, 3, 4},
          new[] {2, 3, 5},
          new[] {2, 4, 5},
          new[] {3, 4, 5},
        };

      Assert.Equal(expected, subsets);
    }

    [Fact]
    public void SubsetsOfSize3Empty()
    {
      var set = new[] { 1, 2};
      var subsets = set.KSubsets(3).ToArray();

      var expected = new int[][] {};
      Assert.Equal(expected, subsets);
    }
  }
}