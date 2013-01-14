namespace Grove.Core.Casting
{
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}