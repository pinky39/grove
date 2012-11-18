namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

  public class DiscardCards : Effect
  {
    public int Count { get; set; }
    public Player Player { get; set; }    

    protected override void ResolveEffect()
    {
      var player = Player ?? Target().Player();

      Game.Enqueue<Decisions.DiscardCards>(
        controller: player,
        init: p => p.Count = Count);
    }
  }
}