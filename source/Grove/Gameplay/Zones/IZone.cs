namespace Grove.Gameplay.Zones
{
  using Card;

  public interface IZone
  {
    Zone Zone { get; }

    void Remove(Card card);
    void AfterAdd(Card card);
    void AfterRemove(Card card);
  }
}