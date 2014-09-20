namespace Grove.Tests.Unit
{
  using System;
  using Xunit;

  public class ClousureFacts
  {
    [Fact]
    public void CaptureMember()
    {
      var scenario = new Scenario();
      scenario.SetResult(5);
      Assert.Equal(5, scenario.GetResult());
    }


    private class Scenario
    {
      private readonly Ballon _ballon;
      private int _value = 1;

      public Scenario()
      {
        _ballon = CreateBallon(() => _value);
      }

      public int GetResult()
      {
        return _ballon.GetHeight();
      }

      public void SetResult(int result)
      {
        _value = result;
      }

      private static Ballon CreateBallon(Func<int> height)
      {
        return new Ballon {GetHeight = height};
      }

      private class Ballon
      {
        public Func<int> GetHeight;
      }
    }
  }
}