namespace Grove.Core.Cards.Effects
{
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
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Controller, 
          init: p => p.Count = DiscardCount);
    }
  }
}