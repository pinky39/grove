namespace Grove.Core.Cards.Casting
{
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}