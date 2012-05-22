namespace Grove.Tests.Unit
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Infrastructure;
  using Xunit;

  public class ShuffleFacts
  {
    [Fact]
    public void SmokeShuffle()
    {
      var original = Enumerable.Range(1, 10000).ToList();
      var shuffled = Enumerable.Range(1, 10000).ToList();
      shuffled.ShuffleInPlace();

      Assert.NotEqual(original, shuffled);
      AssertHaveSameElements(original, shuffled);
    }

    private static void AssertHaveSameElements(IEnumerable<int> first, IEnumerable<int> second)
    {
      Assert.Equal(first.OrderBy(x => x), second.OrderBy(x => x));
    }
  }
}