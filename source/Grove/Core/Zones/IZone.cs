namespace Grove
{
  public interface IZone
  {
    Zone Name { get; }

    void Remove(Card card);
    void AfterRemove(Card card);
    void AfterAdd(Card card);
  }
}