namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class DrawCards : Effect
  {
    public int DiscardCount { get; set; }
    public int DrawCount { get; set; }
    public int Lifeloss { get; set; }

    protected override void ResolveEffect()
    {
      Controller.DrawCards(DrawCount);

      if (Lifeloss > 0)
        Controller.Life -= Lifeloss;

      if (DiscardCount > 0)
        Decisions.Enqueue<DiscardCards>(
          controller: Controller, 
          init: p => p.Count = DiscardCount);
    }
  }
}