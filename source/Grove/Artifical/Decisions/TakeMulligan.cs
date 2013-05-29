namespace Grove.Artifical.Decisions
{
  using System.Linq;

  public class TakeMulligan : Gameplay.Decisions.TakeMulligan
  {
    protected override void ExecuteQuery()
    {
      var landCount = Controller.Hand.Lands.Count();
      Result = landCount < 2 && Controller.Hand.Count > 4;
    }
  }
}