namespace Grove.Ui
{
  public class PlayersInterestChanged
  {
    public bool HasLostInterest { get; set; }

    public object Target { get; set; }
    public object Visual { get; set; }


    public bool InterestedInTarget(object card)
    {
      return Target == card;
    }
  }
}