namespace Grove.Gameplay
{
  public class Permanent : Sorcery
  {
    public override void AfterResolve()
    {
      Card.PutToBattlefield();
    }

    public override bool CanCast()
    {
      if (Card.Has().Flash)
        return true;
      
      return base.CanCast();
    }
  }
}