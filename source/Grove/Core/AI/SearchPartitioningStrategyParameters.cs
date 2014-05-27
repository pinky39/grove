namespace Grove.AI
{
  public class SearchPartitioningStrategyParameters
  {
    public readonly Game GameStateAtBegginingOfSearch;
    public readonly int MoveIndex;
    public readonly ISearchNode Node;
    public readonly Search Search;

    public readonly int SearchDepth;

    public SearchPartitioningStrategyParameters(ISearchNode node, int moveIndex, Search search,
      Game gameStateAtBegginingOfSearch)
    {
      Node = node;
      MoveIndex = moveIndex;
      Search = search;
      GameStateAtBegginingOfSearch = gameStateAtBegginingOfSearch;
      SearchDepth = Node.Game.Turn.StateCount - GameStateAtBegginingOfSearch.Turn.StateCount;
    }
  }
}