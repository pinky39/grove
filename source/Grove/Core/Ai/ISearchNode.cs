namespace Grove.Core.Ai
{
  public interface ISearchNode
  {
    Game Game { get; }
    IPlayer Controller { get; }
    int ResultCount { get; }
    void SetResult(int index);

    void GenerateChoices();
  }
}