namespace Grove.Core.Effects
{
  using Grove.Core.Decisions;
  using Grove.Core.Zones;

  public class PlayersSacrificeLands : Effect
  {
    public int Count;
    public Player OnlyPlayer;

    protected override void ResolveEffect()
    {
      if (OnlyPlayer != null)
      {
        SelectCardsToSacrifice(OnlyPlayer);
        return;
      }

      var players = new[] {Core.Players.Active, Core.Players.Passive};

      foreach (var player in players)
      {
        SelectCardsToSacrifice(player);
      }
    }

    private void SelectCardsToSacrifice(Player player)
    {
      Game.Enqueue<SelectCardsToSacrifice>(
        controller: player,
        init: p =>
          {
            p.MinCount = Count;
            p.MinCount = Count;
            p.Validator = card => card.Is().Land;
            p.Text = FormatText("Select land(s) to sacrifice");
            p.Zone = Zone.Battlefield;
          });
    }
  }
}