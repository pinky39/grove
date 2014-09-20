namespace Grove.UserInterface.CardOrder
{
  public class OrderedObject
  {
    public OrderedObject(object cardOrEffect)
    {
      Object = cardOrEffect;
    }

    public object Object { get; private set; }
    public virtual int? Order { get; set; }
  }
}