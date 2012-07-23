namespace Grove.Core.Messages
{
  public class SearchStarted
  {
    public int SearchDepthLimit { get; set; }
    public int TargetCountLimit { get; set; }
  }
}