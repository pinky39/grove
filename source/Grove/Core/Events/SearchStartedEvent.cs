namespace Grove.Events
{
  using AI;

  public class SearchStartedEvent
  {
    public readonly SearchParameters SearchParameters;

    public SearchStartedEvent(SearchParameters searchParameters)
    {
      SearchParameters = searchParameters;
    }
  }
}