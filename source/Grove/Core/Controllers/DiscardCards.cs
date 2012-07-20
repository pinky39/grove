namespace Grove.Core.Controllers
{
  using Results;

  public abstract class DiscardCards : Decision<ChosenCards>
  {
    public int Count { get; set; }

    protected override bool ShouldExecuteQuery { get { return Count > 0 && Controller.Hand.Count > Count; } }

    public override void ProcessResults()
    {
      if (Count == 0)
        return;

      if (Controller.Hand.Count <= Count)
      {
        Controller.DiscardHand();
        return;
      }

      foreach (var card in Result)
      {                
        card.Discard();
      }
    }
  }
}