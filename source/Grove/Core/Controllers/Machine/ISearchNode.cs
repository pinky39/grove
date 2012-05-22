namespace Mtg.Core.Controllers.Machine
{
  public interface ISearchNode
  {
    bool IsMax { get; }    
    void SetResult(int index);
    int ResultCount { get; }
  }
}