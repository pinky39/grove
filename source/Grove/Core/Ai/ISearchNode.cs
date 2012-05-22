namespace Grove.Core.Ai
{
  public interface ISearchNode
  {
    Game Game { get; }    
    Player Player { get; }
    int ResultCount { get; }
    void SetResult(int index);

    void GenerateChoices();
  }
}