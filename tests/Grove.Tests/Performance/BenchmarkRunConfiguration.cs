namespace Grove.Tests.Performance
{
  public class BenchmarkRunConfiguration
  {
    public readonly string Description;

    public BenchmarkRunConfiguration(string description)
    {
      Description = description;
    }

    public static BenchmarkRunConfiguration Base()
    {
      return new BenchmarkRunConfiguration("Default configuration");
    }

  }
}