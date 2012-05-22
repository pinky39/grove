namespace Grove.Core.Controllers.Machine
{
  public class ConsiderPayingLifeOrMana : Controllers.ConsiderPayingLifeOrMana
  {
    protected override void ExecuteQuery()
    {
      Result = Life != null
        ? Player.Life > 10
        : true;
    }
  }
}