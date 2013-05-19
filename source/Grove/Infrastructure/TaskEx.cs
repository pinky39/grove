namespace Grove.Infrastructure
{
  using System.Threading;
  using System.Threading.Tasks;

  public static class TaskEx
  {
    // source http://stevenhollidge.blogspot.co.uk/2012/06/async-taskdelay.html
    public static Task Delay(int milliseconds)
    {
      var tcs = new TaskCompletionSource<object>();
      new Timer(_ => tcs.SetResult(null)).Change(milliseconds, -1);
      return tcs.Task;
    }
  }
}