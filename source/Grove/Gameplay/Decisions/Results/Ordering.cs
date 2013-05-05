namespace Grove.Gameplay.Decisions.Results
{
  public class Ordering
  {
    public int[] Indices { get; private set; }

    public Ordering(params int[] indices)
    {
      Indices = indices;
    }
  }
}