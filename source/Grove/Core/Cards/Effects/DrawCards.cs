namespace Grove.Core.Cards.Effects
{
  public class DrawCards : Effect
  {
    public int DiscardCount { get; set; }
    public int DrawCount { get; set; }
    public int Lifeloss { get; set; }

    public Player Player { get; set; }

    protected override void ResolveEffect()
    {
      var player = Player ?? Controller;
      
      player.DrawCards(DrawCount);

      if (Lifeloss > 0)
        player.Life -= Lifeloss;

      if (DiscardCount > 0)
        Game.Enqueue<Decisions.DiscardCards>(
          controller: player, 
          init: p => p.Count = DiscardCount);
    }
  }
}