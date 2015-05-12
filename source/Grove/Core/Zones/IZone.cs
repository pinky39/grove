namespace Grove
{
  public interface IZone
  {
    Zone Name { get; }

    void Remove(Card card);    
  }
}