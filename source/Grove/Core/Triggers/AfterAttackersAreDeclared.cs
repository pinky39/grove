namespace Grove.Triggers
{
  using Events;
  using Infrastructure;

  public class AfterAttackersAreDeclared : Trigger, IReceive<AttackersDeclaredEvent>
  {
    private readonly TriggerPredicate _cond;

    private AfterAttackersAreDeclared() {}

    public AfterAttackersAreDeclared(TriggerPredicate cond)
    {
      _cond = cond ?? delegate { return true; };
    }

    public void Receive(AttackersDeclaredEvent e)
    {
      if (_cond(Ctx))
      {
        Set(e);
      }
    }  
  }
}