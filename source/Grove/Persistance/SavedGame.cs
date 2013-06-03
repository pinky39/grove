namespace Grove.Persistance
{
  using System;
  using System.IO;
  using Gameplay;

  [Serializable]
  public class SavedGame
  {    
    public PlayerParameters Player1;
    public PlayerParameters Player2;
    public int RandomSeed;
    public MemoryStream Decisions;
    public int StateCount;
  }
}