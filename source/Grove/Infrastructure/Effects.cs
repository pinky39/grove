namespace Grove.Infrastructure
{
  using System.Threading;

  public class Effects
  {
    public static int DefaultSleepTime { get; set; }

    public static void SleepABit()
    {
      Thread.Sleep(DefaultSleepTime);
    }

    public static void SleepTimesMore(int multiplier)
    {
      Thread.Sleep(DefaultSleepTime*multiplier);
    }
  }
}