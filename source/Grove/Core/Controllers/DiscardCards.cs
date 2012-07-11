namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DiscardCards : Decision<ChosenCards>
  {
    public int Count { get; set; }

    protected override bool ShouldExecuteQuery { get { return Count > 0 && Player.Hand.Count > Count; } }

    public override void ProcessResults()
    {
      if (Count == 0)
        return;

      if (Player.Hand.Count <= Count)
      {
        Player.DiscardHand();
        return;
      }

      foreach (var card in Result)
      {
        Player.DiscardCard(card);
      }
    }
  }
}