namespace Grove.Core.Details.Cards.Effects
{
  using Controllers;

  public class PlayersSacrificeLands : Effect
  {
    public int Count;

    protected override void ResolveEffect()
    {
      var players = new[] {Players.Active, Players.Passive};

      foreach (var player in players)
      {
        Decisions.Enqueue<SacrificePermanents>(
          controller: player,
          init: p =>
            {
              p.Count = Count;
              p.Filter = card => card.Is().Land;
              p.Text = "Select {0} land(s) to sacrifice";
            });
      }
    }
  }
}