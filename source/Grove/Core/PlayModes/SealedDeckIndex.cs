namespace Grove
{
  using System;
  using System.Collections.Generic;

  [Serializable]
  public class SealedDeckIndex
  {
    public readonly Dictionary<string, int> Entries = new Dictionary<string, int>();
  }
}