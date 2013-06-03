namespace Grove.Persistance
{
  using System;
  using System.IO;
  using Gameplay;

  [Serializable]
  public class SavedGame
  {
    public MemoryStream Decisions;
    public PlayerParameters Player1;
    public PlayerParameters Player2;
    public int RandomSeed;
    public int StateCount;
  }
}