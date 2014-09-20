namespace Grove.Infrastructure
{
  public class Snapshot
  {
    public Snapshot(int count)
    {
      History = count;
    }

    public int History { get; set; }
  }
}