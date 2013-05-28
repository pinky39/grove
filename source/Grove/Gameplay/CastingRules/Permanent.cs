namespace Grove.Gameplay.CastingRules
{
  using System;

  [Serializable]
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }
  }
}