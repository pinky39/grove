namespace Grove.Core.Zones
{
  public interface IZone
  {
    Zone Zone { get; }    
    void Remove(Card card, bool moveToOpponentZone);    
  }
}