namespace Grove.Decisions
{
  using System;

  [Serializable]
  public class Ordering
  {
    public Ordering(params int[] indices)
    {
      Indices = indices;
    }

    public int[] Indices { get; private set; }
  }
}