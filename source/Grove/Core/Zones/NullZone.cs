namespace Grove
{
  public class NullZone : IZone
  {
    public Zone Name { get { return Zone.None; } }

    public void Remove(Card card) {}

    public void AfterRemove(Card card) {}

    public void AfterAdd(Card card) {}
  }
}