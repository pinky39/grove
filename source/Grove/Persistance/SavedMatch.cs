namespace Grove.Persistance
{
  using System;

  [Serializable]
  public class SavedMatch
  {
    public SavedGame SavedGame;
    public int Player1WinCount;
    public int Player2WinCount;
  }
}