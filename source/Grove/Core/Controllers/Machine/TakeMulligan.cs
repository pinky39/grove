namespace Grove.Core.Controllers.Machine
{
  using System.Linq;

  public class TakeMulligan : Controllers.TakeMulligan
  {
    protected override void ExecuteQuery()
    {
      var landCount = Player.Hand.Lands.Count();
      Result = landCount < 2 && Player.Hand.Count > 4;
    }
  }
}