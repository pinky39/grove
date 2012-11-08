namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class PlayerSacrificeLands : Effect
  {
    public Player Player { get; set; }
    public int Count { get; set; }
        
    protected override void ResolveEffect()
    {
      Game.Enqueue<SacrificePermanents>(
        controller: Player,
        init: p =>
          {
            p.Count = Count;
            p.Filter = card => card.Is().Land;
            p.Text = "Select {0} land(s) to sacrifice";
          });
    }
  }
}