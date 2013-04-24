namespace Grove.Gameplay.Zones
{
  using Card;

  public class NullZone : IZone
  {
    public Zone Zone { get { return Zone.None; } }

    public void Remove(Card card)
    {      
    }

    public void AfterRemove(Card card)
    {      
    }

    public void AfterAdd(Card card)
    {
      
    }
  }
}