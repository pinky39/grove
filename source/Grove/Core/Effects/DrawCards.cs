namespace Grove.Core.Effects
{
  public class DrawCards : Effect
  {
    public int DiscardCount { get; set; }
    public int DrawCount { get; set; }
    public int Lifeloss { get; set; }

    public override void Resolve()
    {
      Controller.DrawCards(DrawCount);

      if (Lifeloss > 0)
        Controller.Life -= Lifeloss;

      if (DiscardCount > 0)
        Decisions.EnqueueDiscardCards(Controller, DiscardCount);
    }
  }
}