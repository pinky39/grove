namespace Grove.Ai
{
  using Core;
  using Gameplay;
  using Gameplay.Player;

  public interface ISearchNode
  {
    Game Game { get; }
    Player Controller { get; }
    int ResultCount { get; }
    void SetResult(int index);

    void GenerateChoices();
  }
}