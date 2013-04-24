namespace Grove.Core.CastingRules
{
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}