namespace Grove.Core.Decisions.Machine
{
  using System.Linq;

  public class TakeMulligan : Decisions.TakeMulligan
  {
    protected override void ExecuteQuery()
    {
      var landCount = Controller.Hand.Lands.Count();
      Result = landCount < 2 && Controller.Hand.Count > 4;
    }
  }
}