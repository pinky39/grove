namespace Grove.Gameplay.Card.CastingRules
{
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}