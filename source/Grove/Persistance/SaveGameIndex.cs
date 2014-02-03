namespace Grove.Persistance
{
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class SaveGameIndex
  {
    public readonly Dictionary<string, SaveFileInfo> Entries = new Dictionary<string, SaveFileInfo>();
  }
}